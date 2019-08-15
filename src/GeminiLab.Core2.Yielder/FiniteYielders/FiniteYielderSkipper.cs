namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderSkipper<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly int _count;

        private bool _first;

        public FiniteYielderSkipper(IFiniteYielder<T> source, int count) {
            _source = source;
            _count = count;

            _first = true;
        }

        public bool HasNext() {
            if (_first) {
                for (int i = 0; i < _count; ++i) {
                    if (_source.HasNext()) _source.Next();
                    else break;
                }

                _first = false;
            }

            return _source.HasNext();
        }

        public T Next() {
            if (!HasNext()) return default;
            return _source.Next();
        }
    }
}