using System;
using GeminiLab.Core2.Yielder.Yielders;

namespace GeminiLab.Core2.Yielder {
    // return default(TResult) if not accepted
    internal delegate TResult Selector<in TSource, out TResult>(TSource s, out bool accepted);

    public static class Yielder {
        public static IYielder<T> Const<T>(T val) {
            return new ConstYielder<T>(val);
        }

        public static IYielder<T> Repeat<T>(Func<T> fun) {
            return new RepeatYielder<T>(fun);
        }

        public static IYielder<T> Iterate<T>(Func<T, T> fun, T init) {
            return new IterateYielder<T>(fun, init);
        }

        public static IYielder<int> NaturalNumber() {
            return new NYielder();
        }

        public static IYielder<TResult> Map<TSource, TResult>(this IYielder<TSource> source, Func<TSource, TResult> fun) {
            return new YielderMapper<TSource, TResult>(source, fun);
        }

        public static IYielder<T> Filter<T>(this IYielder<T> source, Predicate<T> predicate) {
            return new YielderFilter<T>(source, predicate);
        }

        public static IYielder<T> Skip<T>(this IYielder<T> source, int count) {
            return new YielderSkipper<T>(source, count);
        }

        public static IYielder<TResult> OfType<TSource, TResult>(this IYielder<TSource> source) where TResult : TSource {
            return new YielderOfTypeSelector<TSource, TResult>(source);
        }

        public static IFiniteYielder<T> Take<T>(this IYielder<T> source, int count) {
            return new YielderTaker<T>(source, count);
        }

        public static IFiniteYielder<T> TakeWhile<T>(this IYielder<T> source, Predicate<T> predicate) {
            return new YielderWhileTaker<T>(source, predicate);
        }

        public static IYielder<TResult> Zip<TThis, TThat, TResult>(this IYielder<TThis> source, IYielder<TThat> other,
            Func<TThis, TThat, TResult> func) {
            return new YielderZipper<TThis, TThat, TResult>(source, other, func);
        }
    }
}
