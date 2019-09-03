using System;
using System.Globalization;

namespace GeminiLab.Core2.Logger.Layouts {
    public class ColoredConsoleLayout : ILayout {
        private readonly bool _usingCulture;
        private readonly string? _dateFormat;
        private readonly CultureInfo? _culture;

        public ColoredConsoleLayout(string dateFormat) {
            _dateFormat = dateFormat;
            _usingCulture = false;
        }

        // null means CultureInfo.CurrentCulture
        public ColoredConsoleLayout(CultureInfo? culture = null) {
            _culture = culture ?? CultureInfo.CurrentCulture;
            _usingCulture = true;
        }

        private string getTimeString(DateTime time) {
            return _usingCulture ? time.ToString(_culture) : time.ToString(_dateFormat);
        }

        public static char LevelToColorChar(int level) {
            if (level >= Logger.LevelFatal) return Exconsole.ForegroundDarkRed;
            if (level >= Logger.LevelError) return Exconsole.ForegroundRed;
            if (level >= Logger.LevelWarn) return Exconsole.ForegroundYellow;
            if (level >= Logger.LevelInfo) return Exconsole.ForegroundGreen;
            if (level >= Logger.LevelDebug) return Exconsole.ForegroundMagenta;
            if (level >= Logger.LevelTrace) return Exconsole.ForegroundDarkMagenta;

            return Exconsole.ForegroundBlue;
        }

        public string Format(int level, string category, DateTime time, string content) => $"@v@{LevelToColorChar(level)}[{Logger.LogLevelToString(level)}][{category}][{getTimeString(time)}]@^ {content}";

        public static ColoredConsoleLayout Default { get; } = new ColoredConsoleLayout();
    }
}
