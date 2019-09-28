using System;
using System.Collections.Generic;

namespace GeminiLab.Core2 {
    public static class IntegerExtensions {
        public static IEnumerable<T> Times<T>(this int v, Func<T> fn) {
            if (fn == null) throw new ArgumentNullException(nameof(fn));
            if (v < 0) throw new ArgumentOutOfRangeException(nameof(v));

            for (int i = 0; i < v; ++i) yield return fn();
        }

        public static void Times(this int v, Action act) {
            if (act == null) throw new ArgumentNullException(nameof(act));
            if (v < 0) throw new ArgumentOutOfRangeException(nameof(v));

            for (int i = 0; i < v; ++i) act();
        }

        public static Range To(this int from, int to) => new Range(from, to);
        public static Range To(this int from, int to, int step) => new Range(from, to, step);
    }
}
