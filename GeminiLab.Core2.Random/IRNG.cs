using GeminiLab.Core2.Yielder;

namespace GeminiLab.Core2.Random {
    // contract:
    // Next() return all possible values of TResult
    public interface IRNG<out T> : IYielder<T> { }

    // contract:
    // every class has a constructor like ".ctor()" to initialize it with a runtime-randomly-chosen seed
    public interface IPRNG<out TResult, in TSeed> : IRNG<TResult> {
        void Seed(TSeed seed);
    }

    public interface IPRNG<T> : IPRNG<T, T> { }
}
