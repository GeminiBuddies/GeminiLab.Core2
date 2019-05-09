namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderSelector<TSource, TResult> : IFiniteYielder<TResult> {
        private readonly IFiniteYielder<TSource> _source;
        private readonly Selector<TSource, TResult> _selector;

        private bool _nextCalculated;
        private bool _hasNext;
        private TResult _next;

        public FiniteYielderSelector(IFiniteYielder<TSource> source, Selector<TSource, TResult> selector) {
            _source = source;
            _selector = selector;

            _next = default;
            _nextCalculated = false;
            _hasNext = false;
        }

        public bool HasNext() {
            if (_nextCalculated) return _hasNext;

            _hasNext = false;
            while (_source.HasNext()) {
                _next = _selector(_source.GetNext(), out var accepted);

                if (!accepted) continue;

                _hasNext = true;
                break;
            }

            _nextCalculated = true;
            return _hasNext;
        }

        public TResult GetNext() {
            if (!HasNext()) return default;

            _nextCalculated = false;
            return _next;
        }
    }
}