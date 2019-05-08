namespace GeminiLab.Core2.Yielder {
    public interface IFiniteYielder<out T> {
        bool HasNext();
        T GetNext();
    }
}
