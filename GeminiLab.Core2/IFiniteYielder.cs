namespace GeminiLab.Core2 {
    public interface IFiniteYielder<out T> {
        bool HasNext();
        T GetNext();
    }
}
