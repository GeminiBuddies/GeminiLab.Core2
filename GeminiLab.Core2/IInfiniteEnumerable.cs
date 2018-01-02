namespace GeminiLab.Core2 {
    public interface IInfiniteEnumerable<out T> {
        IInfiniteEnumerator<T> GetEnumerator();
    }

    public interface IInfiniteEnumerator<out T> {
        void Reset();
        T GetNext();
    }
}
