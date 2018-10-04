using System;
using System.Reflection;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using GeminiLab.Core2.Collections.HeapBase;
using GeminiLab.Core2.ML.Json;

namespace TestConsole {
    class Program {
        public static void PrintAssembly(Assembly ass) {
            Console.WriteLine(ass.FullName);
            Console.WriteLine(ass.Location);
            Console.WriteLine(ass.CodeBase);

            foreach (var type in ass.GetTypes()) Console.WriteLine(type);
        }

        public static void Main(string[] args) {
            int a = 0;
            var l = Yielder.Repeat(1).Map(x => ++a).Take(20);
            var v = l.Select(i => i % 3 == 2, i => $"{i}").ToList();
        }
    }
}
