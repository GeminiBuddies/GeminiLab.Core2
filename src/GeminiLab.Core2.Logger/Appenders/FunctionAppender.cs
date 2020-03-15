using System;

namespace GeminiLab.Core2.Logger.Appenders {
    public class FunctionAppender : IAppender {
        private readonly AppenderFunction _func;
        public FunctionAppender(AppenderFunction func) => _func = func;

        public void Append(int level, string category, DateTime time, string content) =>
            _func(level, category, time, content);
    }
}
