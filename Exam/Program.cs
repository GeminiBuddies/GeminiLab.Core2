using System;
using System.Linq;
using System.Reflection;
using GeminiLab.Core2.Logger;
using GeminiLab.Core2.Logger.Appenders;
using GeminiLab.Core2.CommandLineParser;
using System.IO;
using GeminiLab.Core2.Markup.Json;
using Console = GeminiLab.Core2.Exconsole;

namespace Exam {
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

        public static int Main(string[] args) {
            using var loggerContext = new LoggerContext();

            loggerContext.AddCategory("default");
            loggerContext.AddAppender("console", new ColoredConsoleAppender());
            loggerContext.AddAppender("file", new StreamAppender(new FileStream("1.log", FileMode.Append, FileAccess.Write)));
            // loggerContext.AddAppender("console-layout", new ColoredConsoleAppender(new AmazingColorLayout()));
            loggerContext.Connect("default", "console");
            loggerContext.Connect("default", "file", Filters.Threshold(Logger.LevelError));
            // loggerContext.Connect("default", "console-layout", Filters.AcceptFilter);
            var logger = loggerContext.GetLogger("default");

            logger.Info($"cwd: {Environment.CurrentDirectory}");
            logger.Info("printing assemblies...");

            var ass = Assembly.GetExecutingAssembly();
            AssemblyPrinter.PrintAssembly(ass);
            AssemblyPrinter.PrintReferencedAssembly(ass);

            logger.Fatal("fatal");
            logger.Error("error");
            logger.Warn("warn");
            logger.Info("info");
            logger.Debug("debug");
            logger.Trace("trace");

            var x = from i in new[] {1, 2, 3, 4, 5} select i * 2 + 1;

            var o = CommandLineParser<O>.Parse(new[] { "-a", "1233", "-h", "--hh" }, (err, result) => {
                logger.Warn($"{err}");
                return true;
            });
            Console.WriteLine(o.A);
            Console.WriteLine(o.B);

            var a = JsonParser.Parse("{ \"123\": [ 1, 2, 4, false, \"狗粮\" ], \"456\": [ true, null, \"レモン\" ] }");
            Console.Write(a.ToString(JsonStringifyOption.Compact | JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly));
            Console.Write("###");

            return 0;
        }
    }
}
