using System;
using GeminiLab.Core2.ML.Json;

namespace TestConsole {
    class Program {
        public static void Main(string[] args) {
            var str = Console.In.ReadToEnd();

            Console.WriteLine(JsonParser.Parse(str).ToString());
        }
    }
}
