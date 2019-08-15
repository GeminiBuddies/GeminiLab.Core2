namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderTaker<T> : IFiniteYielder<T> {
        private readonly IYielder<T> _source;
        private readonly int _limit;
        private int _count;

        public YielderTaker(IYielder<T> source, int limit) {
            _source = source;
            _limit = limit;

            _count = 0;
        }

        public bool HasNext() {
            return _count < _limit;
        }

        public T Next() {
            if (!HasNext()) return default;

            ++_count;
            return _source.Next();
        }
    }
}
