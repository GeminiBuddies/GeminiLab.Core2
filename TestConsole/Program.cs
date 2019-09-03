using System;
using System.Collections.Generic;
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
using GeminiLab.Core2.CommandLineParser;
using Console = GeminiLab.Core2.Exconsole;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using GeminiLab.Core2.GetOpt;
using GeminiLab.Core2.Logger.Layouts;

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

            logger.Info("testing getopt...");

            var opt = new OptGetter();
            opt.AddOption('h', OptionType.Switch, "help");
            opt.AddOption('m', OptionType.Parameterized, "mark");

            logger.Info("with -- enabled");
            opt.EnableDashDash = true;
            OptGetterTester.TestOptGetter(opt, "-h", "-m123", "-m", "-h123", "-add", "-m", "2", "3", "--help", "--mark", "2", "--", "--help", "qwer", "fff");

            logger.Info("with -- disabled");
            opt.EnableDashDash = false;
            OptGetterTester.TestOptGetter(opt, "-h", "-m123", "-m", "-h123", "-add", "-m", "2", "3", "--help", "--mark", "2", "--", "--help", "qwer", "fff");

            var o = CommandLineParser<O>.Parse(new[] { "-a", "1233", "-h", "--hh" }, (err, result) => {
                logger.Warn($"{err}");
                return true;
            });
            Console.WriteLine(o.A);
            Console.WriteLine(o.B);
        }
    }
}
