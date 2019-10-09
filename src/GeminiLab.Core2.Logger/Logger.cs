using System;

namespace GeminiLab.Core2.Logger {
    public sealed class Logger {
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
            _category.Invoke(level, DateTime.Now, message);
        }

        private const int InternalLevelOff = 0x10000;
        private const int InternalLevelFatal = 0x0c000;
        private const int InternalLevelError = 0x0a000;
        private const int InternalLevelWarn = 0x08000;
        private const int InternalLevelInfo = 0x06000;
        private const int InternalLevelDebug = 0x04000;
        private const int InternalLevelTrace = 0x02000;
        private const int InternalLevelAll = 0x00000;

        public static int LevelOff => InternalLevelOff;
        public static int LevelFatal => InternalLevelFatal;
        public static int LevelError => InternalLevelError;
        public static int LevelWarn => InternalLevelWarn;
        public static int LevelInfo => InternalLevelInfo;
        public static int LevelDebug => InternalLevelDebug;
        public static int LevelTrace => InternalLevelTrace;
        public static int LevelAll => InternalLevelAll;

        public static string LogLevelToString(int level) => level switch {
            InternalLevelFatal => "Fatal",
            InternalLevelError => "Error",
            InternalLevelWarn => "Warn",
            InternalLevelInfo => "Info",
            InternalLevelDebug => "Debug",
            InternalLevelTrace => "Trace",
            _ => $"Level{level}",
        };
    }

}
