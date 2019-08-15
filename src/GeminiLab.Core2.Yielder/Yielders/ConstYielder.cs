namespace GeminiLab.Core2.Yielder.Yielders {
    internal class ConstYielder<T> : IYielder<T> {
        private readonly T _val;

        public ConstYielder(T val) {
            _val = val;
        }

        public T Next() {
            return _val;
        }
    }
}
