using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger.Appenders {
    public class ConsoleAppender : IAppender {
        private readonly ILayout _layout;

        public ConsoleAppender() : this(Layout.DefaultTimedLayout) { }

        public ConsoleAppender(ILayout layout) {
            _layout = layout;
        }

        public void Append(int level, string category, string content) {
            Console.WriteLine(_layout.Format(level, category, content));
        }
    }
}
