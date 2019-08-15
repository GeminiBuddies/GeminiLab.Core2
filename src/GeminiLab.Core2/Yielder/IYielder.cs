namespace GeminiLab.Core2.Yielder {
    public interface IYielder<out T> {
        T Next();
    }
}
