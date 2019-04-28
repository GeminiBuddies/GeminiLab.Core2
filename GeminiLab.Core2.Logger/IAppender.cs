namespace GeminiLab.Core2.Logger {
    // won't implement IDisposable
    // as I don't want to force others to write those nasty codes.
    // implement IDisposable by yourself if you feel it's necessary
    public interface IAppender {
        void Append(int level, string category, string content);
    }
}