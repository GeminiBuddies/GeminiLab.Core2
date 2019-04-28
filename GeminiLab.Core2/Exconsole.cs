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

        public const string SetForeColorBlack = "@k";
        public const string SetForeColorRed = "@r";
        public const string SetForeColorGreen = "@g";
        public const string SetForeColorBlue = "@b";
        public const string SetForeColorCyan = "@c";
        public const string SetForeColorMagenta = "@m";
        public const string SetForeColorYellow = "@y";
        public const string SetForeColorGray = "@a";
        public const string SetForeColorWhite = "@w";
        public const string SetForeColorDarkRed = "@d";
        public const string SetForeColorDarkGreen = "@e";
        public const string SetForeColorDarkBlue = "@u";
        public const string SetForeColorDarkCyan = "@n";
        public const string SetForeColorDarkMagenta = "@t";
        public const string SetForeColorDarkYellow = "@l";
        public const string SetForeColorDarkGray = "@x";
        public const string SetBackColorBlack = "@K";
        public const string SetBackColorRed = "@R";
        public const string SetBackColorGreen = "@G";
        public const string SetBackColorBlue = "@B";
        public const string SetBackColorCyan = "@C";
        public const string SetBackColorMagenta = "@M";
        public const string SetBackColorYellow = "@Y";
        public const string SetBackColorGray = "@A";
        public const string SetBackColorWhite = "@W";
        public const string SetBackColorDarkRed = "@D";
        public const string SetBackColorDarkGreen = "@E";
        public const string SetBackColorDarkBlue = "@U";
        public const string SetBackColorDarkCyan = "@N";
        public const string SetBackColorDarkMagenta = "@T";
        public const string SetBackColorDarkYellow = "@L";
        public const string SetBackColorDarkGray = "@X";
        public const string PushColor = "@v";
        public const string PopColor = "@^";

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
