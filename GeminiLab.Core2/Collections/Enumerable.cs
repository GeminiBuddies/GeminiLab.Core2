using System;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Collections {
    public static class Enumerable {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var i in source) action(i);
        }

        public static IEnumerable<T> EvaluateAll<T>(this IEnumerable<T> source) {
            return new List<T>(source);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Union(y));
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Union(y, comparer));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Intersect(y));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Aggregate((x, y) => x.Intersect(y, comparer));
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var ie in source) {
                if (ie == null) throw new ArgumentOutOfRangeException(nameof(source), "A value in parameter source is null.");

                foreach (var i in ie) yield return i;
            }
        }

        public static IEnumerable<T> RemoveNull<T>(this IEnumerable<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var i in source) if (i != null) yield return i;
        }

        public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int start, int length) {
            var en = source.GetEnumerator();

            for (int i = 0; i < start; ++i) {
                if (en.MoveNext()) continue;

                en.Dispose(); yield break;
            }

            for (int i = 0; i < length; ++i) {
                if (en.MoveNext()) yield return en.Current;
                else { en.Dispose(); yield break; }
            }
        }

        public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int length) => source.Take(0, length);

        public static Dictionary<int, T> NumberItems<T>(this IEnumerable<T> source) => source.NumberItems(0);

        public static Dictionary<int, T> NumberItems<T>(this IEnumerable<T> source, int startIndex) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int count = startIndex;
            return source.ToDictionary(any => count++);
        }
    }

    public static class EnumerableOfString {
        public static string JoinBy(this IEnumerable<string> value, string separator) => string.Join(separator, value);
        public static string JoinBy(this IEnumerable<char> value, string separator) => string.Join(separator, value);

        public static string Join(this IEnumerable<string> value) => string.Join("", value);
        public static string Join(this IEnumerable<char> value) => string.Join("", value);

        public static string Join(this string separator, IEnumerable<string> value) => string.Join(separator, value);
    }
}
