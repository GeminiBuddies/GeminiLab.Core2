using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GeminiLab.Core2 {
    // following are things you really need
    [ExcludeFromCodeCoverage]
    public static partial class Exconsole {
        public static void WriteLineColor(string value) =>
            WriteLineColor(value, null, null);

        public static void WriteLineColor(string value, ConsoleColor? foregroundColor) =>
            WriteLineColor(value, foregroundColor, null);

        public static void WriteLineColor(string value, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor) {
            WriteColor(value, foregroundColor, backgroundColor);
            Console.WriteLine();
        }

        public static void WriteColor(string value) =>
            WriteColor(value, null, null);

        public static void WriteColor(string value, ConsoleColor? foregroundColor) =>
            WriteColor(value, foregroundColor, null);


        public static void WriteColor(string value, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor) {
            var fore = foregroundColor ?? ForegroundColor;
            var back = backgroundColor ?? BackgroundColor;

            ConsoleColor oldFore = ForegroundColor, oldBack = BackgroundColor;
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;

            Console.Write(value);

            Console.ForegroundColor = oldFore;
            Console.BackgroundColor = oldBack;
        }

        public static void WriteLineColorEscaped(string content) {
            WriteColorEscaped(content);
            Console.WriteLine();
        }

        public static void WriteColorEscaped(string content) {
            int len = content.Length;
            char[] buffer = new char[len];
            int ptr = 0;
            void FlushBuffer() {
                if (ptr == 0) return;

                Console.Write(buffer, 0, ptr);
                ptr = 0;
            }

            var colorStack = new Stack<(ConsoleColor Fore, ConsoleColor Back)>();

            for (int i = 0; i < len; ++i) {
                var chr = content[i];
                if (chr == EscapeChar && i < len - 1) {
                    chr = content[++i];

                    if (TryGetColorByChar(chr, out var isFore, out var color)) {
                        FlushBuffer();

                        if (isFore) ForegroundColor = color;
                        else BackgroundColor = color;
                    } else {
                        if (chr == PopColorChar) {
                            if (colorStack.Count > 0) {
                                FlushBuffer();

                                var (fore, back) = colorStack.Pop();
                                ForegroundColor = fore;
                                BackgroundColor = back;
                            }
                        } else if (chr == PushColorChar) {
                            colorStack.Push((ForegroundColor, BackgroundColor));
                        } else if (chr == EscapeChar) {
                            buffer[ptr++] = EscapeChar;
                        } else {
                            buffer[ptr++] = EscapeChar;
                            buffer[ptr++] = chr;
                        }
                    }
                } else {
                    buffer[ptr++] = chr;
                }
            }

            FlushBuffer();
        }

        // auxiliary
        public const char EscapeChar = '@';
        public const char ForegroundBlack = 'k';
        public const char ForegroundRed = 'r';
        public const char ForegroundGreen = 'g';
        public const char ForegroundBlue = 'b';
        public const char ForegroundCyan = 'c';
        public const char ForegroundMagenta = 'm';
        public const char ForegroundYellow = 'y';
        public const char ForegroundGray = 'a';
        public const char ForegroundWhite = 'w';
        public const char ForegroundDarkRed = 'd';
        public const char ForegroundDarkGreen = 'e';
        public const char ForegroundDarkBlue = 'u';
        public const char ForegroundDarkCyan = 'n';
        public const char ForegroundDarkMagenta = 't';
        public const char ForegroundDarkYellow = 'l';
        public const char ForegroundDarkGray = 'x';
        public const char BackgroundBlack = 'K';
        public const char BackgroundRed = 'R';
        public const char BackgroundGreen = 'G';
        public const char BackgroundBlue = 'B';
        public const char BackgroundCyan = 'C';
        public const char BackgroundMagenta = 'M';
        public const char BackgroundYellow = 'Y';
        public const char BackgroundGray = 'A';
        public const char BackgroundWhite = 'W';
        public const char BackgroundDarkRed = 'D';
        public const char BackgroundDarkGreen = 'E';
        public const char BackgroundDarkBlue = 'U';
        public const char BackgroundDarkCyan = 'N';
        public const char BackgroundDarkMagenta = 'T';
        public const char BackgroundDarkYellow = 'L';
        public const char BackgroundDarkGray = 'X';
        public const char PushColorChar = 'v';
        public const char PopColorChar = '^';
        
        private static bool TryGetColorByChar(char chr, out bool fore, out ConsoleColor color) {
            if (!(('a' <= chr && chr <= 'z') || ('A' <= chr && chr <= 'Z'))) {
                fore = default;
                color = default;
                return false;
            }

            fore = (chr & 0x20) != 0;

            color = (chr | (char)0x20) switch {
                ForegroundBlack => ConsoleColor.Black,
                ForegroundRed => ConsoleColor.Red,
                ForegroundGreen => ConsoleColor.Green,
                ForegroundBlue => ConsoleColor.Blue,
                ForegroundCyan => ConsoleColor.Cyan,
                ForegroundMagenta => ConsoleColor.Magenta,
                ForegroundYellow => ConsoleColor.Yellow,
                ForegroundGray => ConsoleColor.Gray,
                ForegroundWhite => ConsoleColor.White,
                ForegroundDarkRed => ConsoleColor.DarkRed,
                ForegroundDarkGreen => ConsoleColor.DarkGreen,
                ForegroundDarkBlue => ConsoleColor.DarkBlue,
                ForegroundDarkCyan => ConsoleColor.DarkCyan,
                ForegroundDarkMagenta => ConsoleColor.DarkMagenta,
                ForegroundDarkYellow => ConsoleColor.DarkYellow,
                ForegroundDarkGray => ConsoleColor.DarkGray,
                _ => (ConsoleColor)(-1),
            };

            if (color != (ConsoleColor)(-1)) return true;

            fore = default;
            color = default;
            return false;
        }
    }
}
