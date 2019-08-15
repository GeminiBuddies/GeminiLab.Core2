using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger.Layouts {
    public class FuncLayout : ILayout {
        private readonly LayoutFunc _func;

        public FuncLayout(LayoutFunc func) => _func = func;

        public string Format(int level, string category, DateTime time, string content) => _func(level, category, time, content);
    }
}
