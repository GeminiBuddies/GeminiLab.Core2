using System;
using System.Globalization;

namespace GeminiLab.Core2.Logger.Layouts {
    internal class DefaultTimedLayout : ILayout {
        public string Format(int level, string category, string content) => $"[{Logger.LogLevelToString(level)}][{category}][{DateTime.Now.ToString(CultureInfo.CurrentCulture)}] {content}";
    }
}