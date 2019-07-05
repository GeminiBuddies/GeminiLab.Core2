using System;
using GeminiLab.Core2;

namespace GeminiLab.Core2.Logger.Appenders {
    public class ColorfulConsoleAppender : IAppender {
        private readonly ILayout _layout;

        public ColorfulConsoleAppender() : this(Layout.DefaultTimedColorfulConsoleLayout) { }

        public ColorfulConsoleAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, string content) {
            Exconsole.WriteLineColorEscaped(_layout.Format(level, category, content));
        }
    }
}
