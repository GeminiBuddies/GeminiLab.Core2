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
using Console = GeminiLab.Core2.Exconsole;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
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
                
                Dictionary<int, int> a = new Dictionary<int, int>();

                for (int i = 0; i < 16; ++i) a[i] = 0;

                for (int i = 0; i < 160000; ++i) {
                    ulong seed0 = 0x0fe12dc34ba56987ul;
                    ulong seed1 = 0x02468acefdb97531ul;

                    unchecked {
                        seed0 ^= (ulong)DateTime.UtcNow.Ticks << 32;
                        seed1 ^= (ulong)DateTime.Now.Ticks << 32;
                        seed0 ^= (ulong)Environment.TickCount;
                        seed1 ^= (ulong)Environment.CurrentDirectory.GetHashCode();

                        for (int x = 0; x < 16; ++x) {
                            seed0 = ((ulong)$"{seed0 - seed1:x16}".GetHashCode() << 32) | (uint)$"{seed0 + seed1:x16}".GetHashCode();
                            seed1 = ((ulong)$"{seed0 + seed1:x16}".GetHashCode() << 32) | (uint)$"{seed1 - seed0:x16}".GetHashCode();
                        }
                    }

                    // Console.WriteLine($"{seed0:x16} {seed1:x16}");
                    a[(int)(seed0 & 0x0f)]++;
                }

                for (int i = 0; i < 16; ++i) Console.WriteLine(a[i]);
            }
        }
    }
}
