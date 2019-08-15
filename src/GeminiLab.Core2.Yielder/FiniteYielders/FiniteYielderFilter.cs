using System;

namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderFilter<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private bool _hasNext;
        private T _next;

        public FiniteYielderFilter(IFiniteYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            _next = default;
            _nextCalculated = false;
            _hasNext = false;
        }

        public bool HasNext() {
            if (_nextCalculated) return _hasNext;

            _hasNext = false;
            while (_source.HasNext()) {
                _next = _source.Next();

                if (!_predicate(_next)) continue;

                _hasNext = true;
                break;
            }

            _nextCalculated = true;
            return _hasNext;
        }

        public T Next() {
            if (!HasNext()) return default;

            _nextCalculated = false;
            return _next;
        }
    }
}