using System;

namespace GeminiLab.Core2.Yielder.FiniteYielders {
    internal class FiniteYielderMapper<TSource, TResult> : IFiniteYielder<TResult> {
        private readonly IFiniteYielder<TSource> _source;
        private readonly Func<TSource, TResult> _fun;

        public FiniteYielderMapper(IFiniteYielder<TSource> source, Func<TSource, TResult> fun) {
            _source = source;
            _fun = fun;
        }

        public bool HasNext() {
            return _source.HasNext();
        }

        public TResult GetNext() {
            return _fun(_source.GetNext());
        }
    }
}