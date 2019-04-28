using System;

namespace GeminiLab.Core2.Logger.Appenders {
    public class StderrAppender : IAppender {
        private readonly ILayout _layout;

        public StderrAppender() : this(Layout.DefaultTimedLayout) { }

        public StderrAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, string content) {
            Console.Error.WriteLine(_layout.Format(level, category, content));
        }
    }
}