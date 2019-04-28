namespace GeminiLab.Core2.Logger.Layouts {
    internal class MinimumLayout : ILayout {
        public string Format(int level, string category, string content) => content;
    }
}
