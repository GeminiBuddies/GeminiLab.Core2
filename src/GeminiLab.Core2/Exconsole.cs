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
                if (chr == InternalEscapeChar && i < len - 1) {
                    chr = content[++i];

                    if (TryGetColorByChar(chr, out var isFore, out var color)) {
                        FlushBuffer();

                        if (isFore) ForegroundColor = color;
                        else BackgroundColor = color;
                    } else {
                        if (chr == InternalPopColorChar) {
                            if (colorStack.Count > 0) {
                                FlushBuffer();

                                var (fore, back) = colorStack.Pop();
                                ForegroundColor = fore;
                                BackgroundColor = back;
                            }
                        } else if (chr == InternalPushColorChar) {
                            colorStack.Push((ForegroundColor, BackgroundColor));
                        } else if (chr == InternalEscapeChar) {
                            buffer[ptr++] = InternalEscapeChar;
                        } else {
                            buffer[ptr++] = InternalEscapeChar;
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
        private const char InternalEscapeChar = '@';
        private const char InternalForegroundBlack = 'k';
        private const char InternalForegroundRed = 'r';
        private const char InternalForegroundGreen = 'g';
        private const char InternalForegroundBlue = 'b';
        private const char InternalForegroundCyan = 'c';
        private const char InternalForegroundMagenta = 'm';
        private const char InternalForegroundYellow = 'y';
        private const char InternalForegroundGray = 'a';
        private const char InternalForegroundWhite = 'w';
        private const char InternalForegroundDarkRed = 'd';
        private const char InternalForegroundDarkGreen = 'e';
        private const char InternalForegroundDarkBlue = 'u';
        private const char InternalForegroundDarkCyan = 'n';
        private const char InternalForegroundDarkMagenta = 't';
        private const char InternalForegroundDarkYellow = 'l';
        private const char InternalForegroundDarkGray = 'x';
        private const char InternalBackgroundBlack = 'K';
        private const char InternalBackgroundRed = 'R';
        private const char InternalBackgroundGreen = 'G';
        private const char InternalBackgroundBlue = 'B';
        private const char InternalBackgroundCyan = 'C';
        private const char InternalBackgroundMagenta = 'M';
        private const char InternalBackgroundYellow = 'Y';
        private const char InternalBackgroundGray = 'A';
        private const char InternalBackgroundWhite = 'W';
        private const char InternalBackgroundDarkRed = 'D';
        private const char InternalBackgroundDarkGreen = 'E';
        private const char InternalBackgroundDarkBlue = 'U';
        private const char InternalBackgroundDarkCyan = 'N';
        private const char InternalBackgroundDarkMagenta = 'T';
        private const char InternalBackgroundDarkYellow = 'L';
        private const char InternalBackgroundDarkGray = 'X';
        private const char InternalPushColorChar = 'v';
        private const char InternalPopColorChar = '^';

        public static char EscapeChar => InternalEscapeChar;
        public static char ForegroundBlack => InternalForegroundBlack;
        public static char ForegroundRed => InternalForegroundRed;
        public static char ForegroundGreen => InternalForegroundGreen;
        public static char ForegroundBlue => InternalForegroundBlue;
        public static char ForegroundCyan => InternalForegroundCyan;
        public static char ForegroundMagenta => InternalForegroundMagenta;
        public static char ForegroundYellow => InternalForegroundYellow;
        public static char ForegroundGray => InternalForegroundGray;
        public static char ForegroundWhite => InternalForegroundWhite;
        public static char ForegroundDarkRed => InternalForegroundDarkRed;
        public static char ForegroundDarkGreen => InternalForegroundDarkGreen;
        public static char ForegroundDarkBlue => InternalForegroundDarkBlue;
        public static char ForegroundDarkCyan => InternalForegroundDarkCyan;
        public static char ForegroundDarkMagenta => InternalForegroundDarkMagenta;
        public static char ForegroundDarkYellow => InternalForegroundDarkYellow;
        public static char ForegroundDarkGray => InternalForegroundDarkGray;
        public static char BackgroundBlack => InternalBackgroundBlack;
        public static char BackgroundRed => InternalBackgroundRed;
        public static char BackgroundGreen => InternalBackgroundGreen;
        public static char BackgroundBlue => InternalBackgroundBlue;
        public static char BackgroundCyan => InternalBackgroundCyan;
        public static char BackgroundMagenta => InternalBackgroundMagenta;
        public static char BackgroundYellow => InternalBackgroundYellow;
        public static char BackgroundGray => InternalBackgroundGray;
        public static char BackgroundWhite => InternalBackgroundWhite;
        public static char BackgroundDarkRed => InternalBackgroundDarkRed;
        public static char BackgroundDarkGreen => InternalBackgroundDarkGreen;
        public static char BackgroundDarkBlue => InternalBackgroundDarkBlue;
        public static char BackgroundDarkCyan => InternalBackgroundDarkCyan;
        public static char BackgroundDarkMagenta => InternalBackgroundDarkMagenta;
        public static char BackgroundDarkYellow => InternalBackgroundDarkYellow;
        public static char BackgroundDarkGray => InternalBackgroundDarkGray;
        public static char PushColorChar => InternalPushColorChar;
        public static char PopColorChar => InternalPopColorChar;

        private static bool TryGetColorByChar(char chr, out bool fore, out ConsoleColor color) {
            if (!(('a' <= chr && chr <= 'z') || ('A' <= chr && chr <= 'Z'))) {
                fore = default;
                color = default;
                return false;
            }

            fore = (chr & 0x20) != 0;

            color = (chr | (char)0x20) switch {
                InternalForegroundBlack => ConsoleColor.Black,
                InternalForegroundRed => ConsoleColor.Red,
                InternalForegroundGreen => ConsoleColor.Green,
                InternalForegroundBlue => ConsoleColor.Blue,
                InternalForegroundCyan => ConsoleColor.Cyan,
                InternalForegroundMagenta => ConsoleColor.Magenta,
                InternalForegroundYellow => ConsoleColor.Yellow,
                InternalForegroundGray => ConsoleColor.Gray,
                InternalForegroundWhite => ConsoleColor.White,
                InternalForegroundDarkRed => ConsoleColor.DarkRed,
                InternalForegroundDarkGreen => ConsoleColor.DarkGreen,
                InternalForegroundDarkBlue => ConsoleColor.DarkBlue,
                InternalForegroundDarkCyan => ConsoleColor.DarkCyan,
                InternalForegroundDarkMagenta => ConsoleColor.DarkMagenta,
                InternalForegroundDarkYellow => ConsoleColor.DarkYellow,
                InternalForegroundDarkGray => ConsoleColor.DarkGray,
                _ => (ConsoleColor)(-1),
            };

            if (color != (ConsoleColor)(-1)) return true;

            fore = default;
            color = default;
            return false;
        }
    }
}
