// for test (and fun) only
namespace GeminiLab.Core2 {
    internal class InfiniteConst<T> : IInfiniteEnumerable<T> {
        private readonly T _val;

        internal InfiniteConst(T value) => _val = value;

        public IInfiniteEnumerator<T> GetEnumerator() => new InfiniteConstEnumerator { Value = _val };

        private class InfiniteConstEnumerator : IInfiniteEnumerator<T> {
            public T Value;
            public T GetNext() => Value;
            public void Reset() { }
        }
    }

    internal class InfiniteNull : IInfiniteEnumerable<object>, IInfiniteEnumerator<object> {
        public IInfiniteEnumerator<object> GetEnumerator() => this;
        public object GetNext() => null;
        public void Reset() { }
    }

    public static class Infinite {
        public static IInfiniteEnumerable<T> Const<T>(T value) => new InfiniteConst<T>(value);
        public static IInfiniteEnumerable<object> Null() => new InfiniteNull();
    }
}
