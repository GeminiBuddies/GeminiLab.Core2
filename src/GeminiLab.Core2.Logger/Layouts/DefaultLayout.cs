using System;

namespace GeminiLab.Core2.Logger.Layouts {
    public class DefaultLayout : ILayout {
        public string Format(int level, string category, DateTime time, string content) => $"[{Logger.LogLevelToString(level)}][{category}][{time:yyyy/MM/dd HH:mm:ss.fff}] {content}";

        public static DefaultLayout Default { get; } = new DefaultLayout();
    }
}
