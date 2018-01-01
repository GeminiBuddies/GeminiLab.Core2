using System;
using System.Collections.Generic;

namespace GeminiLab.Core2 {
    internal class InfiniteEnumerableSelect<TSource, TResult> : IInfiniteEnumerable<TResult> {
        internal IInfiniteEnumerable<TSource> Source;
        internal Func<TSource, TResult> Selector;

        public IInfiniteEnumerator<TResult> GetEnumerator() {
            return new InfiniteEnumerableSelectEnumerator { Enumerator = Source.GetEnumerator(), Selector = Selector };
        }

        private class InfiniteEnumerableSelectEnumerator : IInfiniteEnumerator<TResult> {
            internal IInfiniteEnumerator<TSource> Enumerator;
            internal Func<TSource, TResult> Selector;

            public TResult GetNext() => Selector(Enumerator.GetNext());
            public void Reset() => Enumerator.Reset();
        }
    }

    internal class InfiniteEnumerableZip<TFirst, TSecond, TResult> : IInfiniteEnumerable<TResult> {
        internal IInfiniteEnumerable<TFirst> SourceA;
        internal IInfiniteEnumerable<TSecond> SourceB;
        internal Func<TFirst, TSecond, TResult> Selector;

        public IInfiniteEnumerator<TResult> GetEnumerator() {
            return new InfiniteEnumerableZipEnumerator { EnumeratorA = SourceA.GetEnumerator(), EnumeratorB = SourceB.GetEnumerator(), Selector = Selector };
        }

        private class InfiniteEnumerableZipEnumerator : IInfiniteEnumerator<TResult> {
            internal IInfiniteEnumerator<TFirst> EnumeratorA;
            internal IInfiniteEnumerator<TSecond> EnumeratorB;
            internal Func<TFirst, TSecond, TResult> Selector;

            public TResult GetNext() => Selector(EnumeratorA.GetNext(), EnumeratorB.GetNext());

            public void Reset() {
                EnumeratorA.Reset();
                EnumeratorB.Reset();
            }
        }
    }

    public static class InfiniteEnumerable {
        public static IEnumerable<T> Take<T>(this IInfiniteEnumerable<T> seq, int length) {
            var en = seq.GetEnumerator();

            return length.Times(() => en.GetNext());
        }

        public static IInfiniteEnumerable<TResult> Select<TSource, TResult>(this IInfiniteEnumerable<TSource> seq, Func<TSource, TResult> selector) {
            if (seq == null) throw new ArgumentNullException(nameof(seq));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new InfiniteEnumerableSelect<TSource, TResult> { Source = seq, Selector = selector };
        }

        public static IInfiniteEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IInfiniteEnumerable<TFirst> first, IInfiniteEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector) {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new InfiniteEnumerableZip<TFirst, TSecond, TResult> { SourceA = first, SourceB = second, Selector = selector };
        }
    }
}
