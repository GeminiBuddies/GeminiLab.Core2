using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class RepeatYielder<T> : IYielder<T> {
        private readonly Func<T> _func;

        public RepeatYielder(Func<T> func) => _func = func;

        public T Next() => _func();
    }
}
