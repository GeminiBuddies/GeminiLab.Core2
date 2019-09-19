using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using GeminiLab.Core2.Collections.HeapBase;
using GeminiLab.Core2.Random;
using GeminiLab.Core2.Logger;
using GeminiLab.Core2.Logger.Appenders;
using GeminiLab.Core2.Random.Sugar;
using GeminiLab.Core2.Sugar;
using GeminiLab.Core2.Yielder;
using GeminiLab.Core2.Stream;
using GeminiLab.Core2.CommandLineParser;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using GeminiLab.Core2.Consts;
using GeminiLab.Core2.GetOpt;
using GeminiLab.Core2.IO;
using GeminiLab.Core2.Logger.Layouts;
using GeminiLab.Core2.Markup.Json;
using Console = GeminiLab.Core2.Exconsole;

namespace TestConsole {
    class Program {
        public static void Timed(Action action, string name = "") {
            Console.WriteLine($"- [{name}]Timer begin");
            var now = DateTime.Now;

            action();

            Console.WriteLine($"- [{name}]Timer end at {DateTime.Now - now}");
        }

        public class O {
            [Option(Option = 'a')] public string A { get; set; } = "";

            [Option(LongOption = "bb")] public bool B { get; set; } = false;
        }

        public static void Main(string[] args) {
            using var loggerContext = new LoggerContext();

            loggerContext.AddCategory("default");
            loggerContext.AddAppender("console", new ColoredConsoleAppender());
            loggerContext.AddAppender("file", new StreamAppender(new FileStream("1.log", FileMode.Append, FileAccess.Write)));
            // loggerContext.AddAppender("console-layout", new ColoredConsoleAppender(new AmazingColorLayout()));
            loggerContext.Connect("default", "console");
            loggerContext.Connect("default", "file", Filters.Threshold(Logger.LevelError));
            // loggerContext.Connect("default", "console-layout", Filters.AcceptFilter);
            var logger = loggerContext.GetLogger("default");

            logger.Info(Environment.CurrentDirectory);
            logger.Info("printing assemblies...");

            AssemblyPrinter.PrintAssembly(typeof(Console).Assembly);
            AssemblyPrinter.PrintAssembly(typeof(Logger).Assembly);
            AssemblyPrinter.PrintAssembly(typeof(DefaultRNG).Assembly);
            AssemblyPrinter.PrintAssembly(typeof(IYielder<>).Assembly);
            AssemblyPrinter.PrintAssembly(typeof(IStream).Assembly);
            AssemblyPrinter.PrintAssembly(typeof(OptGetter).Assembly);

            logger.Fatal("fatal");
            logger.Error("error");
            logger.Warn("warn");
            logger.Info("info");
            logger.Debug("debug");
            logger.Trace("trace");

            var o = CommandLineParser<O>.Parse(new[] { "-a", "1233", "-h", "--hh" }, (err, result) => {
                logger.Warn($"{err}");
                return true;
            });
            Console.WriteLine(o.A);
            Console.WriteLine(o.B);

            var a = JsonParser.Parse("{ \"123\": [ 1, 2, 4, false, \"狗粮\" ], \"456\": [ true, null, \"レモン\" ] }");
            Console.Write(a.ToString(JsonStringifyOption.Compact | JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly));
            Console.Write("###");
        }
    }
}
