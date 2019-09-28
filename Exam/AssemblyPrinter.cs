using System;
using System.Reflection;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;

namespace Exam {
    class AssemblyPrinter {
        public static void PrintType(Type type) {
            if (type.IsVisible) {
                Exconsole.WriteColor("V", ConsoleColor.Green);
            } else if (type.IsNotPublic) {
                Exconsole.WriteColor("N", ConsoleColor.Red);
            } else {
                Exconsole.Write("-");
            }

            if (type.IsNested) {
                Exconsole.WriteColor("N", ConsoleColor.Blue);
            } else {
                Exconsole.Write("-");
            }

            if (type.IsGenericType) {
                Exconsole.WriteColor("G", ConsoleColor.Yellow);
            } else {
                Exconsole.Write("-");
            }

            if (type.IsEnum) {
                Exconsole.WriteColor("E", ConsoleColor.DarkCyan);
            } else if (type.IsSubclassOf(typeof(Delegate))) {
                Exconsole.WriteColor("D", ConsoleColor.Yellow);
            } else if (type.IsInterface) {
                Exconsole.WriteColor("I", ConsoleColor.Red);
            } else if (type.IsAbstract && type.IsSealed) {
                Exconsole.WriteColor("S", ConsoleColor.Cyan);
            } else if (type.IsAbstract) {
                Exconsole.WriteColor("A", ConsoleColor.DarkRed);
            } else if (type.IsValueType) {
                Exconsole.WriteColor("V", ConsoleColor.DarkGreen);
            } else if (type.IsSealed) {
                Exconsole.WriteColor("X", ConsoleColor.Green);
            } else {
                Exconsole.Write("-");
            }

            if (type.IsAnsiClass) {
                Exconsole.WriteColor("A", ConsoleColor.DarkRed);
            } else if (type.IsUnicodeClass) {
                Exconsole.WriteColor("U", ConsoleColor.DarkGreen);
            } else if (type.IsAutoClass) {
                Exconsole.WriteColor("T", ConsoleColor.Yellow);
            } else {
                Exconsole.Write("-");
            }

            Exconsole.Write(" ");
            Exconsole.WriteLine(type);
        }

        public static void PrintAssembly(Assembly ass) {
            Exconsole.WriteLineColorEscaped($"Assembly name: @v@r{ass.FullName}@^");
            Exconsole.WriteLineColorEscaped($"Location: @v@g{ass.Location}@^");
            Exconsole.WriteLineColorEscaped($"Code Base: @v@e{ass.CodeBase}@^");

            foreach (var type in ass.GetTypes()) PrintType(type);
        }

        public static void PrintReferencedAssembly(Assembly ass) {
            ass.GetReferencedAssemblies()/*.Where(name => !name.Name.StartsWith("System"))*/.ForEach(name => PrintAssembly(Assembly.Load(name)));
        }
    }
}