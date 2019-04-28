using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger.Layouts {
    internal class ColorfulConsoleLayout : ILayout {
        public string Format(int level, string category, string content) => $"@v@{LevelToColorChar(level)}[{Logger.LogLevelToString(level)}][{category}]@^ {content}";

        public static char LevelToColorChar(int level) {
            if (level >= Logger.LevelFatal) return Exconsole.CharForeColorDarkRed;
            if (level >= Logger.LevelError) return Exconsole.CharForeColorRed;
            if (level >= Logger.LevelWarn) return Exconsole.CharForeColorYellow;
            if (level >= Logger.LevelInfo) return Exconsole.CharForeColorGreen;
            if (level >= Logger.LevelDebug) return Exconsole.CharForeColorMagenta;
            if (level >= Logger.LevelTrace) return Exconsole.CharForeColorDarkMagenta;

            return Exconsole.CharForeColorBlue;
        }
    }
}
