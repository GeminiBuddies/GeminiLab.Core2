using System.Collections;
using System.Collections.Generic;
using GeminiLab.Core2.Sugar;

namespace GeminiLab.Core2.Enumerable {
    public class Slice<T> : IEnumerable<T> {
        private readonly IEnumerable<T> _source;
        private IList<T> _cache;

        private int _start;
        private bool _tillEnd;
        private int _end; // keep it 0 when _tillEnd and _cache unknown
        private int _step;

        private bool _empty;

        private void recalculate() {
            if ((_start < 0 || _end < 0) && _cache == null) {
                // List<T> will know what it is
                _cache = new List<T>(_source);
            }

            if (_start < 0) _start += _cache.Count;
            if (_end < 0) _end += _cache.Count;

            if (_cache != null && (_tillEnd || _end > _cache.Count)) _end = _cache.Count;

            // when will this slice be empty?
            // when _start < 0 or _end < 0 even after added by _cache.Count
            // when _start >= _end when _cache is known or !_tillEnd
            // when _step <= 0
            // when _start >= _cache.Count when _cache is known
            _empty = (_start < 0 || _end < 0 || (_start >= _end && (_cache != null || !_tillEnd)) || _step <= 0 || (_cache != null && _start >= _cache.Count));
        }

        public Slice(IEnumerable<T> source) : this(source, 0, true, 0, 1) { }

        public Slice(IEnumerable<T> source, int start, int end, int step) : this(source, start, false, end, step) { }

        // set end to 0 if tillEnd;
        private Slice(IEnumerable<T> source, int start, bool tillEnd, int end, int step) {
            _source = source;
            if (source is IList<T> sourceList) {
                _cache = sourceList;
            }

            _start = start;
            _tillEnd = tillEnd;
            _end = end;
            _step = step;

            recalculate();
        }

        public Slice<T> Start(int start) {
            _start = start;
            recalculate();

            return this;
        }

        public Slice<T> Start() => Start(0);

        public Slice<T> End(int end) {
            _end = end;
            _tillEnd = false;
            recalculate();

            return this;
        }

        public Slice<T> End() {
            _end = 0;
            _tillEnd = true;
            recalculate();

            return this;
        }

        public Slice<T> Step(int step) {
            _step = step;
            recalculate();

            return this;
        }

        public Slice<T> Step() => Step(1);

        public IEnumerator<T> GetEnumerator() {
            if (_empty) return EmptyEnumerator<T>.Instance;
            if (_cache != null) return new ListSliceEnumerator<T>(_cache, _start, _end, _step);
            return new EnumerableSliceEnumerator<T>(_source, _start, _tillEnd, _end, _step);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class EnumerableSliceEnumerator<T> : IEnumerator<T> {
        private readonly IEnumerable<T> _source;
        private readonly int _start;
        private readonly bool _tillEnd;
        private readonly int _end;
        private readonly int _step;
        private IEnumerator<T> _internalEnumerator;

        private int _pos;
        private bool _first;
        private bool _reachEnd;

        public EnumerableSliceEnumerator(IEnumerable<T> source, int start, bool tillEnd, int end, int step) {
            _source = source;
            _start = start;
            _tillEnd = tillEnd;
            _end = end;
            _step = step;

            Reset();
        }

        public T Current => _internalEnumerator.Current;
        object IEnumerator.Current => Current;

        public void Dispose() { _internalEnumerator.Dispose(); }

        public bool MoveNext() {
            if (_reachEnd) return false;

            if (!_first) {
                (_step - 1).Times(step);
            } else {
                _first = false;
            }

            return stepR();
        }

        public void Reset() {
            _internalEnumerator = _source.GetEnumerator();

            _pos = -1;
            _first = true;
            _reachEnd = false;
            _start.Times(step);
        }

        private void step() => stepR();

        private bool stepR() {
            if (_reachEnd) return false;

            if (++_pos >= _end && !_tillEnd) {
                _reachEnd = true;
                return false;
            }

            return _internalEnumerator.MoveNext();
        }
    }

    internal class ListSliceEnumerator<T> : IEnumerator<T> {
        private readonly IList<T> _source;
        private readonly IEnumerator<int> _indexEnumerator;

        public ListSliceEnumerator(IList<T> source, int start, int end, int step) {
            _source = source;
            var range = new Range(start, end, step);
            _indexEnumerator = range.GetEnumerator();
        }

        public T Current => _source[_indexEnumerator.Current];
        object IEnumerator.Current => Current;

        public void Dispose() => _indexEnumerator.Dispose();
        public bool MoveNext() => _indexEnumerator.MoveNext();
        public void Reset() => _indexEnumerator.Reset();
    }

    public static class SliceExtension {
        public static Slice<T> Slice<T>(this IEnumerable<T> source) {
            return new Slice<T>(source);
        }

        public static Slice<T> Slice<T>(this IEnumerable<T> source, int start, int end, int step) {
            return new Slice<T>(source, start, end, step);
        }
    }
}
