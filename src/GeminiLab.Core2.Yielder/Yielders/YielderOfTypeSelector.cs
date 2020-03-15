namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderOfTypeSelector<TSource, TResult> : IYielder<TResult> {
        private readonly IYielder<TSource> _source;

        public YielderOfTypeSelector(IYielder<TSource> source) {
            _source = source;
        }

        public TResult Next() {
            while (true) {
                if (_source.Next() is TResult res) return res;
            }
        }
    }
}
