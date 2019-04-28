using System;

namespace GeminiLab.Core2.Logger {
    public class Logger {
        private readonly LoggerCategory _category;
        internal Logger(LoggerCategory category) {
            _category = category;
        }

        public void Fatal(string message) => Log(LevelFatal, message);

        public void Error(string message) => Log(LevelError, message);

        public void Warn(string message) => Log(LevelWarn, message);

        public void Info(string message) => Log(LevelInfo, message);

        public void Debug(string message)  => Log(LevelDebug, message);

        public void Trace(string message) => Log(LevelTrace, message);

        public void Log(int level, string message) {
            _category.Invoke(level, message);
        }

        public const int LevelOff = 0x10000;
        public const int LevelFatal = 0x0c000;
        public const int LevelError = 0x0a000;
        public const int LevelWarn = 0x08000;
        public const int LevelInfo = 0x06000;
        public const int LevelDebug = 0x04000;
        public const int LevelTrace = 0x02000;
        public const int LevelAll = 0x00000;

        public static string LogLevelToString(int level) {
            switch (level) {
            case LevelFatal:
                return "Fatal";
            case LevelError:
                return "Error";
            case LevelWarn:
                return "Warn";
            case LevelInfo:
                return "Info";
            case LevelDebug:
                return "Debug";
            case LevelTrace:
                return "Trace";
            default:
                return $"Level{level}";
            }
        }
    }

}
