using System;
using System.Globalization;

namespace GeminiLab.Core2.Logger.Layouts {
    internal class ColorfulTimedConsoleLayout : ILayout {
        private readonly bool _usingCulture;
        private readonly string _dateFormat;
        private readonly CultureInfo _culture;

        public ColorfulTimedConsoleLayout(string dateFormat) {
            _dateFormat = dateFormat;
            _usingCulture = false;
        }

        // null means CultureInfo.CurrentCulture
        public ColorfulTimedConsoleLayout(CultureInfo culture = null) {
            _culture = culture ?? CultureInfo.CurrentCulture;
            _usingCulture = true;
        }

        private string getTimeString() {
            if (_usingCulture) {
                return DateTime.Now.ToString(_culture);
            } else {
                return DateTime.Now.ToString(_dateFormat);
            }
        }

        public string Format(int level, string category, string content) => $"@v@{ColorfulConsoleLayout.LevelToColorChar(level)}[{Logger.LogLevelToString(level)}][{category}][{getTimeString()}]@^ {content}";
    }
}
