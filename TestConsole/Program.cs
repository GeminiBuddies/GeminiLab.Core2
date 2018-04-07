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
            Console.WriteLine("6Iuf5Yip5Zu95a6255Sf5q275Lul".DecodeBase64());

            var a = new int[10] {9, 2, 3, 1, 6, 4, 8, 0, 7, 5};
            a.MakeHeap(10);
            a.SortHeap(10);
        }
    }
}
