using GeminiLab.Core2;

namespace GeminiLab.Core2.Logger.Layouts {
    internal class ColorfulConsoleLayout : ILayout {
        public string Format(int level, string category, string content) => $"@v@{LevelToColorChar(level)}[{Logger.LogLevelToString(level)}][{category}]@^ {content}";

        public static char LevelToColorChar(int level) {
            if (level >= Logger.LevelFatal) return Exconsole.ForegroundDarkRed;
            if (level >= Logger.LevelError) return Exconsole.ForegroundRed;
            if (level >= Logger.LevelWarn) return Exconsole.ForegroundYellow;
            if (level >= Logger.LevelInfo) return Exconsole.ForegroundGreen;
            if (level >= Logger.LevelDebug) return Exconsole.ForegroundMagenta;
            if (level >= Logger.LevelTrace) return Exconsole.ForegroundDarkMagenta;

            return Exconsole.ForegroundBlue;
        }
    }
}
