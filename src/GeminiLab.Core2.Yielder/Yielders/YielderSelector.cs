namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderSelector<TSource, TResult> : IYielder<TResult> {
        private readonly IYielder<TSource> _source;
        private readonly Selector<TSource, TResult> _selector;

        public YielderSelector(IYielder<TSource> source, Selector<TSource, TResult> selector) {
            _source = source;
            _selector = selector;
        }

        public TResult Next() {
            while (true) {
                var val = _source.Next();
                var res = _selector(val, out var accepted);

                if (accepted) return res;
            }
        }
    }
}
