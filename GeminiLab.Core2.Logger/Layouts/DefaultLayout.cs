namespace GeminiLab.Core2.Logger.Layouts {
    internal class DefaultLayout : ILayout {
        public string Format(int level, string category, string content) => $"[{Logger.LogLevelToString(level)}][{category}] {content}";
    }
}
