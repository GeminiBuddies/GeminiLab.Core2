using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2 {
    public static partial class Exconsole {

        public static void Write(string value) {
            if (UseColorfulOutput) {
                WriteColorful(value);
            } else {
                WriteRaw(value);
            }
        }

        public static void Write(string format, params object[] arg) {
            Write(string.Format(format, arg));
        }

        public static void WriteLine(string value) {
            Write(value);
            Console.Write(Console.Out.NewLine);
        }

        public static void WriteLine(string format, params object[] arg) { 
            WriteLine(string.Format(format, arg));
        }

        // following are things you really need
        public static bool UseColorfulOutput { get; set; } = true;

        public static void WriteRaw(string value) => Console.Write(value);

        public static void WriteColorful(string content) {
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
                if (chr == CharEscape && i < len - 1) {
                    chr = content[++i];

                    if (tryGetColorByChar(chr, out var isFore, out var color)) {
                        FlushBuffer();

                        if (isFore) ForegroundColor = color;
                        else BackgroundColor = color;
                    } else {
                        if (chr == CharPopColor) {
                            if (colorStack.Count > 0) {
                                FlushBuffer();

                                var (fore, back) = colorStack.Pop();
                                ForegroundColor = fore;
                                BackgroundColor = back;
                            }
                        } else if (chr == CharPushColor) {
                            colorStack.Push((ForegroundColor, BackgroundColor));
                        } else if (chr == CharEscape) {
                            buffer[ptr++] = CharEscape;
                        } else {
                            buffer[ptr++] = CharEscape;
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
        public const char CharEscape = '@';
        public const char CharForeColorBlack = 'k';
        public const char CharForeColorRed = 'r';
        public const char CharForeColorGreen = 'g';
        public const char CharForeColorBlue = 'b';
        public const char CharForeColorCyan = 'c';
        public const char CharForeColorMagenta = 'm';
        public const char CharForeColorYellow = 'y';
        public const char CharForeColorGray = 'a';
        public const char CharForeColorWhite = 'w';
        public const char CharForeColorDarkRed = 'd';
        public const char CharForeColorDarkGreen = 'e';
        public const char CharForeColorDarkBlue = 'u';
        public const char CharForeColorDarkCyan = 'n';
        public const char CharForeColorDarkMagenta = 't';
        public const char CharForeColorDarkYellow = 'l';
        public const char CharForeColorDarkGray = 'x';
        public const char CharBackColorBlack = 'K';
        public const char CharBackColorRed = 'R';
        public const char CharBackColorGreen = 'G';
        public const char CharBackColorBlue = 'B';
        public const char CharBackColorCyan = 'C';
        public const char CharBackColorMagenta = 'M';
        public const char CharBackColorYellow = 'Y';
        public const char CharBackColorGray = 'A';
        public const char CharBackColorWhite = 'W';
        public const char CharBackColorDarkRed = 'D';
        public const char CharBackColorDarkGreen = 'E';
        public const char CharBackColorDarkBlue = 'U';
        public const char CharBackColorDarkCyan = 'N';
        public const char CharBackColorDarkMagenta = 'T';
        public const char CharBackColorDarkYellow = 'L';
        public const char CharBackColorDarkGray = 'X';
        public const char CharPushColor = 'v';
        public const char CharPopColor = '^';

        private static bool tryGetColorByChar(char chr, out bool fore, out ConsoleColor color) {
            switch (chr) {
            case CharForeColorBlack:
                fore = true; color = ConsoleColor.Black; return true;
            case CharForeColorRed:
                fore = true; color = ConsoleColor.Red; return true;
            case CharForeColorGreen:
                fore = true; color = ConsoleColor.Green; return true;
            case CharForeColorBlue:
                fore = true; color = ConsoleColor.Blue; return true;
            case CharForeColorCyan:
                fore = true; color = ConsoleColor.Cyan; return true;
            case CharForeColorMagenta:
                fore = true; color = ConsoleColor.Magenta; return true;
            case CharForeColorYellow:
                fore = true; color = ConsoleColor.Yellow; return true;
            case CharForeColorGray:
                fore = true; color = ConsoleColor.Gray; return true;
            case CharForeColorWhite:
                fore = true; color = ConsoleColor.White; return true;
            case CharForeColorDarkRed:
                fore = true; color = ConsoleColor.DarkRed; return true;
            case CharForeColorDarkGreen:
                fore = true; color = ConsoleColor.DarkGreen; return true;
            case CharForeColorDarkBlue:
                fore = true; color = ConsoleColor.DarkBlue; return true;
            case CharForeColorDarkCyan:
                fore = true; color = ConsoleColor.DarkCyan; return true;
            case CharForeColorDarkMagenta:
                fore = true; color = ConsoleColor.DarkMagenta; return true;
            case CharForeColorDarkYellow:
                fore = true; color = ConsoleColor.DarkYellow; return true;
            case CharForeColorDarkGray:
                fore = true; color = ConsoleColor.DarkGray; return true;
            case CharBackColorBlack:
                fore = false; color = ConsoleColor.Black; return true;
            case CharBackColorRed:
                fore = false; color = ConsoleColor.Red; return true;
            case CharBackColorGreen:
                fore = false; color = ConsoleColor.Green; return true;
            case CharBackColorBlue:
                fore = false; color = ConsoleColor.Blue; return true;
            case CharBackColorCyan:
                fore = false; color = ConsoleColor.Cyan; return true;
            case CharBackColorMagenta:
                fore = false; color = ConsoleColor.Magenta; return true;
            case CharBackColorYellow:
                fore = false; color = ConsoleColor.Yellow; return true;
            case CharBackColorGray:
                fore = false; color = ConsoleColor.Gray; return true;
            case CharBackColorWhite:
                fore = false; color = ConsoleColor.White; return true;
            case CharBackColorDarkRed:
                fore = false; color = ConsoleColor.DarkRed; return true;
            case CharBackColorDarkGreen:
                fore = false; color = ConsoleColor.DarkGreen; return true;
            case CharBackColorDarkBlue:
                fore = false; color = ConsoleColor.DarkBlue; return true;
            case CharBackColorDarkCyan:
                fore = false; color = ConsoleColor.DarkCyan; return true;
            case CharBackColorDarkMagenta:
                fore = false; color = ConsoleColor.DarkMagenta; return true;
            case CharBackColorDarkYellow:
                fore = false; color = ConsoleColor.DarkYellow; return true;
            case CharBackColorDarkGray:
                fore = false; color = ConsoleColor.DarkGray; return true;
            default:
                fore = false; color = ConsoleColor.Black; return false;
            }
        }
    }
}
