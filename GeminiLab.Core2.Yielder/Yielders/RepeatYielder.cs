using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class RepeatYielder<T> : IYielder<T> {
        private readonly Func<T> _iterator;

        public RepeatYielder(Func<T> iterator) {
            _iterator = iterator;
        }

        public T GetNext() {
            return _iterator();
        }
    }
}
