using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections.Comparers {
    public static class Comparers {
        public static IComparer<T> Lambda<T>(Func<T, T, bool> lessThan) => new LambdaComparer<T>(lessThan);
        public static IComparer<T> Lambda<T>(Func<T, T, int> lambda) => new LambdaComparer<T>(lambda);

        public static IComparer<T> AsComparer<T>(this Func<T, T, bool> lessThan) => Lambda(lessThan);
        public static IComparer<T> AsComparer<T>(this Func<T, T, int> lambda) => Lambda(lambda);

        public static IComparer<T> Reverse<T>(this IComparer<T> comparer) => new ReverseComparer<T>(comparer);
    }
}
