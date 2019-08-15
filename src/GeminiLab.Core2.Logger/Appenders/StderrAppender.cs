using System;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger.Appenders {
    public class StderrAppender : IAppender {
        private readonly ILayout _layout;

        public StderrAppender() : this(DefaultLayout.Default) { }

        public StderrAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, DateTime time, string content) {
            Console.Error.WriteLine(_layout.Format(level, category, time, content));
        }
    }
}