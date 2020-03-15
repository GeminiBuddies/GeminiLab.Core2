using System;

namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderWhileTaker<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private T _next = default!;
        private bool _nextGood;

        private bool _reachEnd;

        private void clearNext() {
            _nextCalculated = _nextGood = false;
            _next = default!;
        }

        public FiniteYielderWhileTaker(IFiniteYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            _reachEnd = false;
            clearNext();
        }

        public bool HasNext() {
            if (_reachEnd) return false;
            if (_nextCalculated) return _nextGood;

            if (!_source.HasNext()) {
                _reachEnd = true;
                return false;
            }

            _next = _source.Next();
            _nextCalculated = true;
            _nextGood = _predicate(_next);

            return _nextGood;
        }

        public T Next() {
            if (!HasNext()) throw new InvalidOperationException();

            T rv = _next;
            clearNext();
            return rv;
        }
    }
}