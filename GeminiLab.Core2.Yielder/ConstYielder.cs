namespace GeminiLab.Core2.Yielder {
    internal class ConstYielder<T> : IYielder<T> {
        private readonly T _val;

        public ConstYielder(T val) {
            _val = val;
        }

        public T GetNext() {
            return _val;
        }
    }
}
