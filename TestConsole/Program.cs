using System;
using GeminiLab.Core2.ML.Json;

namespace TestConsole {
    class Program {
        public static void Main(string[] args) {
            var str = Console.In.ReadToEnd();

            var jsonValue = JsonParser.Parse(str);
            Console.WriteLine(jsonValue.ToString());
            Console.WriteLine(jsonValue.ToStringMinimized(false));
            Console.WriteLine(jsonValue.ToStringPrettified(false));
        }
    }
}
