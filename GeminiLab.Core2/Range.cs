using System;
using System.Collections;
using System.Collections.Generic;

namespace GeminiLab.Core2 {
    // python style range
    // [start, end)
    public class Range : IEnumerable<int> {
        private readonly int _start, _end, _step;

        public Range() : this((int) 0, (int) 0, (int) 1) { }
        public Range(int end) : this((int) 0, end) { }
        public Range(int start, int end) : this(start, end, start <= end ? 1 : -1) { }
        public Range(int start, int end, int step) {
            _start = start;
            _end = end;
            _step = step;

            if (step == 0) throw new ArgumentException(nameof(step));
            if (start != end && Math.Sign(step) != Math.Sign(end - start)) throw new ArgumentException(nameof(step));
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new RangeEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new RangeEnumerator(this);

        internal class RangeEnumerator : IEnumerator<int> {
            private bool _used;
            private readonly Range _mutter;

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext() {
                if (!_used) Current = _mutter._start;
                else Current += _mutter._step;

                _used = true;
                
                return (Current < _mutter._end && _mutter._step > 0) || (Current > _mutter._end && _mutter._step < 0);
            }

            public void Reset() => _used = false;
            public RangeEnumerator(Range mutter) { _used = false; _mutter = mutter; }
        }
    }

    public static class RangeExtensions {
        public static Range To(this int from, int to) => new Range(from, to);
        public static Range To(this int from, int to, int step) => new Range(from, to, step);
    }
}
