using System;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger.Appenders {
    public class StdoutAppender : IAppender {
        private readonly ILayout _layout;

        public StdoutAppender() : this(DefaultLayout.Default) { }

        public StdoutAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, DateTime time, string content) {
            Console.WriteLine(_layout.Format(level, category, time, content));
        }
    }
}
