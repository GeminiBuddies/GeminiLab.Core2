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
using Console = GeminiLab.Core2.Exconsole;

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
            Console.WriteLineColorEscaped($"Code Base: @v@g{ass.CodeBase}@^");

            foreach (var type in ass.GetTypes()) PrintType(type);
        }

        public static void Main(string[] args) {
            var loggerContext = new LoggerContext();
            loggerContext.AddCategory("default");
            loggerContext.AddAppender("console", new ColorfulConsoleAppender());
            loggerContext.Connect("default", "console");
            var logger = loggerContext.GetLogger("default");

            logger.Info("printing assemblies");
            
            PrintAssembly(typeof(Console).Assembly);
            PrintAssembly(typeof(Logger).Assembly);
            PrintAssembly(typeof(DefaultRNG).Assembly);
            PrintAssembly(typeof(IYielder<>).Assembly);
            
            var l = Yielder.NaturalNumber().Take(20);
            var v = l.Filter(i => i % 3 == 2).Map(i => $"{i}").ToList();
            var chooser = "rgbcmywdeuntlx".MakeChooser();
            Console.WriteLineColorEscaped($"{v.Select(x => $"@v@{chooser.Next()}{x}@^").JoinBy(", ")}");

            logger.Warn("warning!");

            logger.Fatal("fatal!");
            logger.Error("error!");
            logger.Debug("debug into");
            logger.Trace("trace");
            logger.Log(1024, "custom level");

            Console.WriteLineColorEscaped($"@v{Console.SetForeColorDarkRed}FATAL {Console.SetForeColorRed}ERROR {Console.SetForeColorYellow}WARN@^");
            Console.WriteLineColorEscaped($"@v{Console.SetForeColorDarkGreen}INFO {Console.SetForeColorMagenta}DEBUG {Console.SetForeColorDarkMagenta}TRACE@^");

            int times = 2000000;
            int range = 100;
            int[] count = new int[range];
            times.Times(() => DefaultRNG.Instance.Next(100)).ForEach(x => ++count[x]);

            count.NumberItems().ForEach(x => Console.WriteLine($"{x.Key}: {x.Value * range / 1.0 / times}"));
        }
    }
}
