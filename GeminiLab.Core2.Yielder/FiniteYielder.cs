using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Yielder {
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

    internal class FiniteYielderFilter<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private bool _hasNext;
        private T _next;

        public FiniteYielderFilter(IFiniteYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            _next = default;
            _nextCalculated = false;
            _hasNext = false;
        }

        public bool HasNext() {
            if (_nextCalculated) return _hasNext;

            _hasNext = false;
            while (_source.HasNext()) {
                _next = _source.GetNext();

                if (!_predicate(_next)) continue;

                _hasNext = true;
                break;
            }

            _nextCalculated = true;
            return _hasNext;
        }

        public T GetNext() {
            if (!HasNext()) return default;

            _nextCalculated = false;
            return _next;
        }
    }

    internal class FiniteYielderSelector<TSource, TResult> : IFiniteYielder<TResult> {
        private readonly IFiniteYielder<TSource> _source;
        private readonly Selector<TSource, TResult> _selector;

        private bool _nextCalculated;
        private bool _hasNext;
        private TResult _next;

        public FiniteYielderSelector(IFiniteYielder<TSource> source, Selector<TSource, TResult> selector) {
            _source = source;
            _selector = selector;

            _next = default;
            _nextCalculated = false;
            _hasNext = false;
        }

        public bool HasNext() {
            if (_nextCalculated) return _hasNext;

            _hasNext = false;
            while (_source.HasNext()) {
                _next = _selector(_source.GetNext(), out var accepted);

                if (!accepted) continue;

                _hasNext = true;
                break;
            }

            _nextCalculated = true;
            return _hasNext;
        }

        public TResult GetNext() {
            if (!HasNext()) return default;

            _nextCalculated = false;
            return _next;
        }
    }

    internal class FiniteYielderSkipper<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly int _count;

        private bool _first;

        public FiniteYielderSkipper(IFiniteYielder<T> source, int count) {
            _source = source;
            _count = count;

            _first = true;
        }

        public bool HasNext() {
            if (_first) {
                for (int i = 0; i < _count; ++i) {
                    if (_source.HasNext()) _source.GetNext();
                    else break;
                }

                _first = false;
            }

            return _source.HasNext();
        }

        public T GetNext() {
            if (!HasNext()) return default;
            return _source.GetNext();
        }
    }

    internal class FiniteYielderTaker<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly int _limit;
        private int _count;

        public FiniteYielderTaker(IFiniteYielder<T> source, int limit) {
            _source = source;
            _limit = limit;

            _count = 0;
        }

        public bool HasNext() {
            return _source.HasNext() && _count < _limit;
        }

        public T GetNext() {
            if (!_source.HasNext()) return default;

            ++_count;
            return _source.GetNext();
        }
    }

    internal class FiniteYielderWhileTaker<T> : IFiniteYielder<T> {
        private readonly IFiniteYielder<T> _source;
        private readonly Predicate<T> _predicate;

        private bool _nextCalculated;
        private T _next;
        private bool _nextGood;

        private bool _reachEnd;

        private void clearNext() {
            _nextCalculated = _nextGood = false;
            _next = default;
        }

        public FiniteYielderWhileTaker(IFiniteYielder<T> source, Predicate<T> predicate) {
            _source = source;
            _predicate = predicate;

            _reachEnd = false;
            clearNext();
        }

        public bool HasNext() {
            if (_reachEnd) return false;
            if (_nextCalculated) return _nextGood;

            if (!_source.HasNext()) {
                _reachEnd = true;
                return false;
            }

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

    public static class FiniteYielder {
        public static IFiniteYielder<T> Repeat<T>(T val, int count) {
            return Yielder.Repeat(val).Take(count);
        }

        public static bool All<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            while (source.HasNext()) {
                if (!predicate(source.GetNext())) return false;
            }

            return true;
        }

        public static bool Any<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            while (source.HasNext()) {
                if (predicate(source.GetNext())) return true;
            }

            return false;
        }

        public static int Count<T>(this IFiniteYielder<T> source) {
            int count = 0;
            while (source.HasNext()) {
                checked{ count++; }
            }

            return count;
        }

        public static bool Contains<T>(this IFiniteYielder<T> source, T item) {
            return Contains(source, item, EqualityComparer<T>.Default);
        }

        public static bool Contains<T>(this IFiniteYielder<T> source, T item, IEqualityComparer<T> comp) {
            while (source.HasNext()) {
                if (comp.Equals(item, source.GetNext())) return true;
            }

            return false;
        }

        public static IFiniteYielder<TResult> Map<TSource, TResult>(this IFiniteYielder<TSource> source, Func<TSource, TResult> fun) {
            return new FiniteYielderMapper<TSource, TResult>(source, fun);
        }

        public static IFiniteYielder<T> Filter<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            return new FiniteYielderFilter<T>(source, predicate);
        }

        public static IFiniteYielder<TResult> Select<TSource, TResult>(this IFiniteYielder<TSource> source, Predicate<TSource> filter, Func<TSource, TResult> map) {
            // ReSharper disable once AssignmentInConditionalExpression
            return new FiniteYielderSelector<TSource, TResult>(source, (TSource s, out bool accepted) => (accepted = filter(s)) ? map(s) : default);
        }

        public static IFiniteYielder<TResult> Select<TSource, TResult>(this IFiniteYielder<TSource> source, Selector<TSource, TResult> selector) {
            return new FiniteYielderSelector<TSource, TResult>(source, selector);
        }

        public static IFiniteYielder<T> Skip<T>(this IFiniteYielder<T> source, int count) {
            return new FiniteYielderSkipper<T>(source, count);
        }

        public static IFiniteYielder<TResult> OfType<TSource, TResult>(this IFiniteYielder<TSource> source) where TResult : TSource {
            return new FiniteYielderSelector<TSource, TResult>(source, (TSource s, out bool accepted) => {
                if (s is TResult res) {
                    accepted = true;
                    return res;
                }

                accepted = false;
                return default;
            });
        }

        public static IFiniteYielder<T> Take<T>(this IFiniteYielder<T> source, int count) {
            return new FiniteYielderTaker<T>(source, count);
        }

        public static IFiniteYielder<T> TakeWhile<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            return new FiniteYielderWhileTaker<T>(source, predicate);
        }

        public static List<T> ToList<T>(this IFiniteYielder<T> source) {
            var rv = new List<T>();

            while (source.HasNext()) rv.Add(source.GetNext());
            return rv;
        }
    }
}
