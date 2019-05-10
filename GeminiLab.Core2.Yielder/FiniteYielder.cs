using System;
using System.Collections.Generic;
using GeminiLab.Core2.Yielder.FiniteYielders;

namespace GeminiLab.Core2.Yielder {
    public static class FiniteYielder {
        public static IFiniteYielder<T> Const<T>(T val, int count) {
            return Yielder.Const(val).Take(count);
        }
        
        public static bool All<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            while (source.HasNext()) {
                if (!predicate(source.Next())) return false;
            }

            return true;
        }

        public static bool Any<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            while (source.HasNext()) {
                if (predicate(source.Next())) return true;
            }

            return false;
        }

        public static int Count<T>(this IFiniteYielder<T> source) {
            int count = 0;
            while (source.HasNext()) {
                checked{ count++; }
                source.Next();
            }

            return count;
        }

        public static bool Contains<T>(this IFiniteYielder<T> source, T item) {
            return Contains(source, item, EqualityComparer<T>.Default);
        }

        public static bool Contains<T>(this IFiniteYielder<T> source, T item, IEqualityComparer<T> comp) {
            while (source.HasNext()) {
                if (comp.Equals(item, source.Next())) return true;
            }

            return false;
        }

        public static IFiniteYielder<TResult> Map<TSource, TResult>(this IFiniteYielder<TSource> source, Func<TSource, TResult> fun) {
            return new FiniteYielderMapper<TSource, TResult>(source, fun);
        }

        public static IFiniteYielder<T> Filter<T>(this IFiniteYielder<T> source, Predicate<T> predicate) {
            return new FiniteYielderFilter<T>(source, predicate);
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

            while (source.HasNext()) rv.Add(source.Next());
            return rv;
        }
    }
}
