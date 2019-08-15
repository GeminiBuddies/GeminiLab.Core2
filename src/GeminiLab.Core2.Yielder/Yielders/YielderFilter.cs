using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderFilter<T> : IYielder<T> {
        private readonly IYielder<T> _source;
        private readonly Predicate<T> _predicate;

        public YielderFilter(IYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;
        }

        public T Next() {
            while (true) {
                T temp = _source.Next();
                    
                if (_predicate(temp)) return temp;
            }
        }
    }
}
