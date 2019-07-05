using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace GeminiLab.Core2 {
    // following are things you really need
    public static partial class Exconsole {
        public static void WriteLineColor(string value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null) {
            WriteColor(value, foregroundColor, backgroundColor);
            Console.WriteLine();
        }

        public static void WriteColor(string value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null) {
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

                    if (tryGetColorByChar(chr, out var isFore, out var color)) {
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
        
        private static bool tryGetColorByChar(char chr, out bool fore, out ConsoleColor color) {
            switch (chr) {
            case ForegroundBlack:
                fore = true; color = ConsoleColor.Black; return true;
            case ForegroundRed:
                fore = true; color = ConsoleColor.Red; return true;
            case ForegroundGreen:
                fore = true; color = ConsoleColor.Green; return true;
            case ForegroundBlue:
                fore = true; color = ConsoleColor.Blue; return true;
            case ForegroundCyan:
                fore = true; color = ConsoleColor.Cyan; return true;
            case ForegroundMagenta:
                fore = true; color = ConsoleColor.Magenta; return true;
            case ForegroundYellow:
                fore = true; color = ConsoleColor.Yellow; return true;
            case ForegroundGray:
                fore = true; color = ConsoleColor.Gray; return true;
            case ForegroundWhite:
                fore = true; color = ConsoleColor.White; return true;
            case ForegroundDarkRed:
                fore = true; color = ConsoleColor.DarkRed; return true;
            case ForegroundDarkGreen:
                fore = true; color = ConsoleColor.DarkGreen; return true;
            case ForegroundDarkBlue:
                fore = true; color = ConsoleColor.DarkBlue; return true;
            case ForegroundDarkCyan:
                fore = true; color = ConsoleColor.DarkCyan; return true;
            case ForegroundDarkMagenta:
                fore = true; color = ConsoleColor.DarkMagenta; return true;
            case ForegroundDarkYellow:
                fore = true; color = ConsoleColor.DarkYellow; return true;
            case ForegroundDarkGray:
                fore = true; color = ConsoleColor.DarkGray; return true;
            case BackgroundBlack:
                fore = false; color = ConsoleColor.Black; return true;
            case BackgroundRed:
                fore = false; color = ConsoleColor.Red; return true;
            case BackgroundGreen:
                fore = false; color = ConsoleColor.Green; return true;
            case BackgroundBlue:
                fore = false; color = ConsoleColor.Blue; return true;
            case BackgroundCyan:
                fore = false; color = ConsoleColor.Cyan; return true;
            case BackgroundMagenta:
                fore = false; color = ConsoleColor.Magenta; return true;
            case BackgroundYellow:
                fore = false; color = ConsoleColor.Yellow; return true;
            case BackgroundGray:
                fore = false; color = ConsoleColor.Gray; return true;
            case BackgroundWhite:
                fore = false; color = ConsoleColor.White; return true;
            case BackgroundDarkRed:
                fore = false; color = ConsoleColor.DarkRed; return true;
            case BackgroundDarkGreen:
                fore = false; color = ConsoleColor.DarkGreen; return true;
            case BackgroundDarkBlue:
                fore = false; color = ConsoleColor.DarkBlue; return true;
            case BackgroundDarkCyan:
                fore = false; color = ConsoleColor.DarkCyan; return true;
            case BackgroundDarkMagenta:
                fore = false; color = ConsoleColor.DarkMagenta; return true;
            case BackgroundDarkYellow:
                fore = false; color = ConsoleColor.DarkYellow; return true;
            case BackgroundDarkGray:
                fore = false; color = ConsoleColor.DarkGray; return true;
            default:
                fore = false; color = ConsoleColor.Black; return false;
            }
        }
    }
}
