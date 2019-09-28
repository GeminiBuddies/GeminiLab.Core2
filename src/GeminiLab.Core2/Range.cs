using System.Collections;
using System.Collections.Generic;
using GeminiLab.Core2.Collections;

namespace GeminiLab.Core2 {
    // python style range
    // [start, end)
    // 
    // if start == end || sign(step) != sign(end - start) || step == 0 then
    //     yield nothing
    public class Range : IEnumerable<int> {
        public int Start { get; }
        public int End { get; }
        public int Step { get; }

        private readonly bool _invalid;

        public Range() : this(0, 0, 1) { }

        public Range(int end) : this(0, end, 1) { }

        public Range(int start, int end) : this(start, end, 1) { }

        public Range(int start, int end, int step) {
            Start = start;
            End = end;
            Step = step;

            _invalid = (Start == End) || (Step == 0) || ((step > 0) ^ (end > start));
        }

        public IEnumerator<int> GetEnumerator() {
            return _invalid ? (IEnumerator<int>)EmptyEnumerator<int>.Instance : new RangeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class RangeEnumerator : IEnumerator<int> {
        private bool _used;
        private bool _dead;
        private readonly int _start, _end, _step;
        private readonly bool _ascending;

        public RangeEnumerator(Range mutter) {
            _start = mutter.Start;
            _end = mutter.End;
            _step = mutter.Step;

            _ascending = _step > 0;
            Reset();
        }

        public int Current { get; private set; }
        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext() {
            if (_dead) return false;

            if (!_used) {
                Current = _start;
                _used = true;
            } else {
                int old = Current;
                Current = unchecked(Current += _step);

                // overflowed:      (Current > old) ^ _ascending
                // end reached:     Current == _end
                // out of range:    (Current < _end) ^ _ascending
                _dead = ((Current > old) ^ _ascending) || (Current == _end) || ((Current < _end) ^ _ascending);
            }

            return !_dead;
        }

        public void Reset() {
            _used = false;
            _dead = false;
        }
    }
}
