namespace GeminiLab.Core2.Random {
    // contract:
    // every class implementing this interface has a constructor ".ctor()" to initialize itself with a runtime-randomly-chosen seed
    public interface IPRNG<out TResult, in TSeed> : IRNG<TResult> {
        void Seed(TSeed seed);
    }

    public interface IPRNG<T> : IPRNG<T, T> { }
}
