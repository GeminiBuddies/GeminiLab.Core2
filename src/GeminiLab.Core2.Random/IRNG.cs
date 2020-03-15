using GeminiLab.Core2.Yielder;

namespace GeminiLab.Core2.Random {
    public interface IRNG<out T> : IYielder<T> { }
}