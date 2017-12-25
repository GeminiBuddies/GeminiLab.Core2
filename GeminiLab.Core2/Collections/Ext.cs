using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections {
    public static class Ext {
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
    }
}
