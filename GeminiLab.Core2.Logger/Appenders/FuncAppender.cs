using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger.Appenders {
    public class FuncAppender : IAppender {
        private readonly AppenderFunc _func;
        public FuncAppender(AppenderFunc func) => _func = func;

        public void Append(int level, string category, DateTime time, string content) =>
            _func(level, category, time, content);
    }
}
