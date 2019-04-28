namespace GeminiLab.Core2.Logger {
    public interface IFilter {
        bool Accept(int level, string content);
    }
}