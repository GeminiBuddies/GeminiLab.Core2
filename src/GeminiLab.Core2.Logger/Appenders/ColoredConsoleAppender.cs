using System;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger.Appenders {
    public class ColoredConsoleAppender : IAppender {
        private readonly ILayout _layout;

        public ColoredConsoleAppender() : this(ColoredConsoleLayout.Default) { }

        public ColoredConsoleAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, DateTime time, string content) {
            Exconsole.WriteLineColorEscaped(_layout.Format(level, category, time, content));
        }
    }
}
