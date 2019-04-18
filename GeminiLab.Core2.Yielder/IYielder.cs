namespace GeminiLab.Core2 {
    public interface IYielder<out T> {
        T GetNext();
    }
}
