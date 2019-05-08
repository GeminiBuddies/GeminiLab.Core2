using System;
using System.Diagnostics;
using System.Linq;

namespace GeminiLab.Core2.Yielder {
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

    internal class YielderFilter<T> : IYielder<T> {
        private readonly IYielder<T> _source;
        private readonly Predicate<T> _predicate;

        public YielderFilter(IYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;
        }

        public T GetNext() {
            while (true) {
                T temp = _source.GetNext();
                    
                if (_predicate(temp)) return temp;
            }
        }
    }

    internal class YielderSelector<TSource, TResult> : IYielder<TResult> {
        private readonly IYielder<TSource> _source;
        private readonly Selector<TSource, TResult> _selector;

        public YielderSelector(IYielder<TSource> source, Selector<TSource, TResult> selector) {
            _source = source;
            _selector = selector;
        }

        public TResult GetNext() {
            while (true) {
                var val = _source.GetNext();
                var res = _selector(val, out var accepted);

                if (accepted) return res;
            }
        }
    }

    internal class YielderSkipper<T> : IYielder<T> {
        private readonly IYielder<T> _source;
        private readonly int _count;
        private bool _first = true;

        public YielderSkipper(IYielder<T> source, int count) {
            _source = source;
            _count = count;
        }

        public T GetNext() {
            if (_first) {
                for (int i = 0; i < _count; ++i) _source.GetNext();
                _first = false;
            }

            return _source.GetNext();
        }
    }

    internal class YielderTaker<T> : IFiniteYielder<T> {
        private readonly IYielder<T> _source;
        private readonly int _limit;
        private int _count;

        public YielderTaker(IYielder<T> source, int limit) {
            _source = source;
            _limit = limit;

            _count = 0;
        }

        public bool HasNext() {
            return _count < _limit;
        }

        public T GetNext() {
            if (!HasNext()) return default;

            ++_count;
            return _source.GetNext();
        }
    }

    internal class YielderWhileTaker<T> : IFiniteYielder<T> {
        private readonly IYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private T _next;
        private bool _nextGood;

        private void clearNext() {
            _nextCalculated = _nextGood = false;
            _next = default;
        }

        public YielderWhileTaker(IYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            clearNext();
        }

        public bool HasNext() {
            if (_nextCalculated) return _nextGood;

            _next = _source.GetNext();
            _nextCalculated = true;
            _nextGood = _predicate(_next);

            return _nextGood;
        }

        public T GetNext() {
            if (!HasNext()) return default;

            T rv = _next;
            clearNext();
            return rv;
        }
    }
    
    internal class ConstYielder<T> : IYielder<T> {
        private readonly T _val;

        public ConstYielder(T val) {
            _val = val;
        }

        public T GetNext() {
            return _val;
        }
    }

    internal class IterateYielder<T> : IYielder<T> {
        private readonly Func<T> _iterator;

        public IterateYielder(Func<T> iterator) {
            _iterator = iterator;
        }

        public T GetNext() {
            return _iterator();
        }
    }

    // return default(TResult) if not accepted
    public delegate TResult Selector<in TSource, out TResult>(TSource s, out bool accepted);

    public static class Yielder {
        public static IYielder<T> Repeat<T>(T val) {
            return new ConstYielder<T>(val);
        }

        public static IYielder<T> Iterate<T>(Func<T> iterator) {
            return new IterateYielder<T>(iterator);
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
            return new YielderSelector<TSource, TResult>(source, (TSource s, out bool accepted) => {
                if (s is TResult res) {
                    accepted = true;
                    return res;
                }

                accepted = false;
                return default;
            });
        }

        public static IFiniteYielder<T> Take<T>(this IYielder<T> source, int count) {
            return new YielderTaker<T>(source, count);
        }

        public static IFiniteYielder<T> TakeWhile<T>(this IYielder<T> source, Predicate<T> predicate) {
            return new YielderWhileTaker<T>(source, predicate);
        }
    }
}
