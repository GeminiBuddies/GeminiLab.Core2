using System;
using System.Globalization;

namespace GeminiLab.Core2.Logger.Layouts {
    internal class ColorfulTimedConsoleLayout : ILayout {
        public string Format(int level, string category, string content) => $"@v@{ColorfulConsoleLayout.LevelToColorChar(level)}[{Logger.LogLevelToString(level)}][{category}][{DateTime.Now.ToString(CultureInfo.CurrentCulture)}]@^ {content}";
    }
}