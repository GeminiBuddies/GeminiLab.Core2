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
using GeminiLab.Core2.Logger.Layouts;

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
            public string Format(int level, string category, DateTime time, string content) {
                var sb = new StringBuilder("##@v");
                var chooser1 = (@"XDEUNTL").MakeChooser();
                var chooser2 = (@"rgbcmywa").MakeChooser();
                var str = $"[{Logger.LogLevelToString(level)}][{category}][{time:yyyy/MM/dd HH:mm:ss.fff}]";

                foreach (var c in str) {
                    sb.Append($"@{chooser1.Next()}@{chooser2.Next()}{c}");
                }

                sb.Append("@^");
                return sb.ToString();
            }
        }
        
        public static void Main(string[] args) {
            using (var loggerContext = new LoggerContext()) {
                loggerContext.AddCategory("default");
                loggerContext.AddAppender("console", new ColoredConsoleAppender());
                loggerContext.AddAppender("file", new StreamAppender(new FileStream("1.log", FileMode.Append, FileAccess.Write)));
                // loggerContext.AddAppender("console-layout", new ColoredConsoleAppender(new AmazingColorLayout()));
                loggerContext.Connect("default", "console");
                loggerContext.Connect("default", "file", Filters.Threshold(Logger.LevelError));
                // loggerContext.Connect("default", "console-layout", Filters.AcceptFilter);
                var logger = loggerContext.GetLogger("default");

                logger.Info(Environment.CurrentDirectory);
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
                logger.Log(Logger.LevelError + 1, "custom level");

                Console.WriteLineColorEscaped($"@v@dFATAL @rERROR @yWARN@^;");
                Console.WriteLineColorEscaped($"@v@eINFO @mDEBUG @tTRACE@^;");
            }
        }
    }
}
