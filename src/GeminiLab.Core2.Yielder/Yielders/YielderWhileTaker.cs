using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderWhileTaker<T> : IFiniteYielder<T> {
        private readonly IYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private T _next;
        private bool _nextGood;

        private void clearNext() {
            _nextCalculated = _nextGood = false;
            _next = default;
        }

        public YielderWhileTaker(IYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            clearNext();
        }

        public bool HasNext() {
            if (_nextCalculated) return _nextGood;

            _next = _source.Next();
            _nextCalculated = true;
            _nextGood = _predicate(_next);

            return _nextGood;
        }

        public T Next() {
            if (!HasNext()) return default;

            T rv = _next;
            clearNext();
            return rv;
        }
    }
}
