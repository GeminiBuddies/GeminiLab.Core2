using System;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Collections {
    public static class EnumerableExtensions {
        public static void Consume<T>(this IEnumerable<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var x in source) { }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var i in source) action(i);
        }

        /*
        public static IEnumerable<T> EvaluateAll<T>(this IEnumerable<T> source) {
            return new List<T>(source);
        }
        */

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Enumerable.Union);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Union(y, comparer));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Enumerable.Intersect);
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Intersect(y, comparer));
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var ie in source) {
                if (ie == null) throw new ArgumentOutOfRangeException(nameof(source));

                foreach (var i in ie) yield return i;
            }
        }

        public static IEnumerable<T> RemoveNull<T>(this IEnumerable<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var i in source) if (i != null) yield return i;
        }

        public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int start, int length) {
            // C# dispose it, but ReSharper don't think so
            // ReSharper disable once GenericEnumeratorNotDisposed
            using var en = source?.GetEnumerator() ?? throw new ArgumentNullException(nameof(source));

            for (int i = 0; i < start; ++i) {
                if (!en.MoveNext()) yield break;
            }

            for (int i = 0; i < length; ++i) {
                if (en.MoveNext()) yield return en.Current;
                else yield break;
            }
        }

        public static IDictionary<int, T> NumberItems<T>(this IEnumerable<T> source) => source.NumberItems(0);

        public static IDictionary<int, T> NumberItems<T>(this IEnumerable<T> source, int startIndex) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int count = startIndex;
            return source.ToDictionary(any => count++);
        }

        public static bool All(this IEnumerable<bool> source) {
            using var en = source?.GetEnumerator() ?? throw new ArgumentNullException(nameof(source));

            while (en.MoveNext()) {
                if (!en.Current) return false;
            }

            return true;
        }

        public static bool Any(this IEnumerable<bool> source) {
            using var en = source?.GetEnumerator() ?? throw new ArgumentNullException(nameof(source));

            while (en.MoveNext()) {
                if (en.Current) return true;
            }

            return false;
        }
    }
}
