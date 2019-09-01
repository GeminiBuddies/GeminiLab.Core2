namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderOfTypeSelector<TSource, TResult> : IFiniteYielder<TResult> {
        private readonly IFiniteYielder<TSource> _source;

        private bool _nextCalculated;
        private bool _hasNext;
        private TResult _next;

        public FiniteYielderOfTypeSelector(IFiniteYielder<TSource> source) {
            _source = source;

            _next = default;
            _nextCalculated = false;
            _hasNext = false;
        }

        public bool HasNext() {
            if (_nextCalculated) return _hasNext;

            _hasNext = false;
            while (_source.HasNext()) {
                if (!(_source.Next() is TResult t)) continue;

                _next = t;
                _hasNext = true;
                break;
            }

            _nextCalculated = true;
            return _hasNext;
        }

        public TResult Next() {
            if (!HasNext()) return default;

            _nextCalculated = false;
            return _next;
        }
    }
}
