using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class IterateYielder<T> : IYielder<T> {
        private readonly Func<T, T> _fun;
        private T _val;

        public IterateYielder(Func<T, T> fun, T init) {
            _fun = fun;
            _val = init;
        }

        public T Next() => _val = _fun(_val);
    }
}
