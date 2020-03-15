using System;

namespace GeminiLab.Core2.Logger.Layouts {
    public class FunctionLayout : ILayout {
        private readonly LayoutFunction _func;

        public FunctionLayout(LayoutFunction func) => _func = func;

        public string Format(int level, string category, DateTime time, string content) => _func(level, category, time, content);
    }
}
