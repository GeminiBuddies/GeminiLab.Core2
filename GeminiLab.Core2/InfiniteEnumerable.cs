using System;
using System.Collections.Generic;

namespace GeminiLab.Core2 {
    #region "support classes"
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

    internal class InfiniteEnumerableAppendHead<T> : IInfiniteEnumerable<T> {
        internal IInfiniteEnumerable<T> Source;
        internal IEnumerable<T> Head;

        public IInfiniteEnumerator<T> GetEnumerator() {
            return new InfiniteEnumerableAppendHeadEnumerator(this);
        }

        private class InfiniteEnumerableAppendHeadEnumerator : IInfiniteEnumerator<T> {
            private readonly InfiniteEnumerableAppendHead<T> _mother;
            private bool _isIEnumerableFinished;

            private IEnumerator<T> _enumerator;
            private IInfiniteEnumerator<T> _infiniteEnumerator;

            public T GetNext() {
                if (_isIEnumerableFinished) return _infiniteEnumerator.GetNext();

                if (_enumerator == null) _enumerator = _mother.Head.GetEnumerator();
                if (_enumerator.MoveNext()) return _enumerator.Current;

                _infiniteEnumerator = _mother.Source.GetEnumerator();
                _isIEnumerableFinished = true;
                return _infiniteEnumerator.GetNext();
            }

            public void Reset() {
                _isIEnumerableFinished = false;
                _enumerator = null;
                _infiniteEnumerator = null;
            }

            internal InfiniteEnumerableAppendHeadEnumerator(InfiniteEnumerableAppendHead<T> mother) {
                _mother = mother;
                Reset();
            }
        }
    }
    #endregion

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

        public static IInfiniteEnumerable<T> AppendHead<T>(this IInfiniteEnumerable<T> seq, IEnumerable<T> content) {
            if (seq == null) throw new ArgumentNullException(nameof(seq));
            if (content == null) throw new ArgumentNullException(nameof(content));

            return new InfiniteEnumerableAppendHead<T> { Source = seq, Head = content };
        }
    }
}
