using System.Collections;
using System.Collections.Generic;

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
        private readonly bool _ascending;

        private readonly bool _invalid;

        public Range() : this(0) { }
        public Range(int end) : this(0, end) { }
        public Range(int start, int end) : this(start, end, start <= end ? 1 : -1) { }
        public Range(int start, int end, int step) {
            Start = start;
            End = end;
            Step = step;

            _ascending = step > 0;

            _invalid = (Start == End) || (Step == 0) || ((step > 0) ^ (end > start));
        }

        public IEnumerator<int> GetEnumerator() {
            if (_invalid) return EmptyEnumerator<int>.Instance;

            return new RangeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        private class RangeEnumerator : IEnumerator<int> {
            private bool _used;
            private bool _dead;
            private readonly Range _mutter;

            public RangeEnumerator(Range mutter) {
                _mutter = mutter;
                Reset();
            }

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext() {
                if (_dead) return false;

                if (!_used) {
                    Current = _mutter.Start;
                    _used = true;
                } else {
                    int old = Current;
                    Current = unchecked(Current += _mutter.Step);

                    _dead = ((Current > old) ^ _mutter._ascending)                // overflowed,
                            || (Current == _mutter.End)                           // end reached,
                            || ((Current < _mutter.End) ^ _mutter._ascending)     // out of range
                    ;

                    return !_dead;
                }

                return !_dead;
            }

            public void Reset() {
                _used = false;
                _dead = false;
            }
        }
    }

    public static class RangeExtensions {
        public static Range To(this int from, int to) => new Range(from, to);
        public static Range To(this int from, int to, int step) => new Range(from, to, step);
    }
}
