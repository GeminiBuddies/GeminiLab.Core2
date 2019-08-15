namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderSkipper<T> : IYielder<T> {
        private readonly IYielder<T> _source;
        private readonly int _count;
        private bool _first = true;

        public YielderSkipper(IYielder<T> source, int count) {
            _source = source;
            _count = count;
        }

        public T Next() {
            if (_first) {
                for (int i = 0; i < _count; ++i) _source.Next();
                _first = false;
            }

            return _source.Next();
        }
    }
}
