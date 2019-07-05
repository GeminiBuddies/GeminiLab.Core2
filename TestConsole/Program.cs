using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using GeminiLab.Core2.Collections.HeapBase;
using GeminiLab.Core2.ML.Json;
using GeminiLab.Core2.Random;
using GeminiLab.Core2.Logger;
using GeminiLab.Core2.Logger.Appenders;
using GeminiLab.Core2.Random.Sugar;
using GeminiLab.Core2.Sugar;
using GeminiLab.Core2.Yielder;
using GeminiLab.Core2.Stream;
using Console = GeminiLab.Core2.Exconsole;
using System.IO;
using System.Text;

namespace TestConsole {
    class Program {
        public static void PrintType(Type type) {
            if (type.IsVisible) {
                Console.WriteColor("V", ConsoleColor.Green);
            } else if (type.IsNotPublic) {
                Console.WriteColor("N", ConsoleColor.Red);
            } else {
                Console.Write("-");
            }

            if (type.IsNested) {
                Console.WriteColor("N", ConsoleColor.Blue);
            } else {
                Console.Write("-");
            }

            if (type.IsGenericType) {
                Console.WriteColor("G", ConsoleColor.Yellow);
            } else {
                Console.Write("-");
            }

            if (type.IsEnum) {
                Console.WriteColor("E", ConsoleColor.DarkCyan);
            } else if (type.IsSubclassOf(typeof(Delegate))) {
                Console.WriteColor("D", ConsoleColor.Yellow);
            } else if (type.IsInterface) {
                Console.WriteColor("I", ConsoleColor.Red);
            } else if (type.IsAbstract && type.IsSealed) {
                Console.WriteColor("S", ConsoleColor.Cyan);
            } else if (type.IsAbstract) {
                Console.WriteColor("A", ConsoleColor.DarkRed);
            } else if (type.IsValueType) {
                Console.WriteColor("V", ConsoleColor.DarkGreen);
            } else if (type.IsSealed) {
                Console.WriteColor("X", ConsoleColor.Green);
            } else {
                Console.Write("-");
            }

            if (type.IsAnsiClass) {
                Console.WriteColor("A", ConsoleColor.DarkRed);
            } else if (type.IsUnicodeClass) {
                Console.WriteColor("U", ConsoleColor.DarkGreen);
            } else {
                Console.Write("-");
            }

            Console.Write(" ");
            Console.WriteLine(type);
        }

        public static void PrintAssembly(Assembly ass) {
            Console.WriteLineColorEscaped($"Assembly name: @v@r{ass.FullName}@^");
            Console.WriteLineColorEscaped($"Location: @v@g{ass.Location}@^");
            Console.WriteLineColorEscaped($"Code Base: @v@e{ass.CodeBase}@^");

            foreach (var type in ass.GetTypes()) PrintType(type);
        }

        /// <summary>Why c++ have no anonymous inner classes?</summary>
        public class AmazingColorLayout : ILayout {
            public string Format(int level, string category, string content) {
                var sb = new StringBuilder("##@v");
                var chooser = ("" + Console.ForegroundBlue
                                 + Console.ForegroundRed
                                 + Console.ForegroundGreen
                                 + Console.ForegroundCyan
                                 + Console.ForegroundMagenta
                                 + Console.ForegroundYellow
                                 + Console.ForegroundDarkBlue
                                 + Console.ForegroundDarkRed
                                 + Console.ForegroundDarkGreen
                                 + Console.ForegroundDarkCyan
                                 + Console.ForegroundDarkMagenta
                                 + Console.ForegroundDarkYellow
                                 + Console.ForegroundGray
                                 + Console.ForegroundWhite).MakeChooser();

                foreach (var c in
                    $"[{Logger.LogLevelToString(level)}][{category}][{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}]") {
                    sb.Append($"@{chooser.Next()}{c}");
                }

                sb.Append("@^");
                return sb.ToString();
            }
        }

        public static void Main(string[] args) {
            var loggerContext = new LoggerContext();
            loggerContext.AddCategory("default");
            loggerContext.AddAppender("console", new ColorfulConsoleAppender());
            loggerContext.AddAppender("stderr", new StderrAppender());
            loggerContext.AddAppender("console-layout", new ColorfulConsoleAppender(new AmazingColorLayout()));
            loggerContext.Connect("default", "console");
            loggerContext.Connect("default", "stderr", Filters.Threshold(Logger.LevelError));
            loggerContext.Connect("default", "console-layout", Filters.AcceptFilter);
            var logger = loggerContext.GetLogger("default");

            logger.Info("printing assemblies");

            PrintAssembly(typeof(Console).Assembly);
            PrintAssembly(typeof(Logger).Assembly);
            PrintAssembly(typeof(DefaultRNG).Assembly);
            PrintAssembly(typeof(IYielder<>).Assembly);
            PrintAssembly(typeof(IStream).Assembly);

            logger.Warn("warning!");

            logger.Fatal("fatal!");
            logger.Error("error!");
            logger.Debug("debug into");
            logger.Trace("trace");
            logger.Log(1024, "custom level");

            Console.WriteLineColorEscaped($"@v@dFATAL @rERROR @yWARN@^;");
            Console.WriteLineColorEscaped($"@v@eINFO @mDEBUG @tTRACE@^;");
        }
    }
}
