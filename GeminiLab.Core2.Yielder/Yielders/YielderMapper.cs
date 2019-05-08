using System;

namespace GeminiLab.Core2.Yielder.Yielders {
    internal class YielderMapper<TSource, TResult> : IYielder<TResult> {
        private readonly IYielder<TSource> _source;
        private readonly Func<TSource, TResult> _fun;

        public YielderMapper(IYielder<TSource> source, Func<TSource, TResult> fun) {
            _fun = fun;
            _source = source;
        }

        public TResult GetNext() {
            return _fun(_source.GetNext());
        }
    }
}
