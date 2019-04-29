using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace GeminiLab.Core2 {
    public static partial class Exconsole {
        public static ConsoleColor BackgroundColor {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public static int BufferHeight {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        public static int BufferWidth {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        public static bool CapsLock => Console.CapsLock;

        public static int CursorLeft {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        public static int CursorSize {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        public static bool CursorVisible {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        public static TextWriter Error => Console.Error;

        public static ConsoleColor ForegroundColor {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public static TextReader In => Console.In;

        public static Encoding InputEncoding {
            get => Console.InputEncoding;
            set => Console.InputEncoding = value;
        }

        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        public static bool IsInputRedirected => Console.IsInputRedirected;

        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        public static bool KeyAvailable => Console.KeyAvailable;

        public static int LargestWindowHeight => Console.LargestWindowHeight;

        public static int LargestWindowWidth => Console.LargestWindowWidth;

        public static bool NumberLock => Console.NumberLock;

        public static TextWriter Out => Console.Out;

        public static Encoding OutputEncoding {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        public static string Title {
            get => Console.Title;
            set => Console.Title = value;
        }

        public static bool TreatControlCAsInput {
            get => Console.TreatControlCAsInput;
            set => Console.TreatControlCAsInput = value;
        }

        public static int WindowHeight {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        public static int WindowLeft {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        public static int WindowTop {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        public static int WindowWidth {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        public static void Beep() => Console.Beep();

        public static void Beep(int frequency, int duration) => Console.Beep(frequency, duration);

        public static void Clear() => Console.Clear();

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, 
            int targetLeft, int targetTop) 
            => Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight,
            int targetLeft, int targetTop, char sourceChar, System.ConsoleColor sourceForeColor,
            System.ConsoleColor sourceBackColor) 
            => Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, 
                sourceChar, sourceForeColor, sourceBackColor);

        public static Stream OpenStandardError() => Console.OpenStandardError();

        public static Stream OpenStandardError(int bufferSize) => Console.OpenStandardError(bufferSize);

        public static Stream OpenStandardInput() => Console.OpenStandardInput();

        public static Stream OpenStandardInput(int bufferSize) => Console.OpenStandardInput(bufferSize);

        public static Stream OpenStandardOutput() => Console.OpenStandardOutput();

        public static Stream OpenStandardOutput(int bufferSize) => Console.OpenStandardOutput(bufferSize);

        public static int Read() => Console.Read();

        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        public static string ReadLine() => Console.ReadLine();

        public static void ResetColor() => Console.ResetColor();

        public static void SetBufferSize(int width, int height) => Console.SetBufferSize(width, height);

        public static void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        public static void SetError(TextWriter newError) => Console.SetError(newError);

        public static void SetIn(TextReader newIn) => Console.SetIn(newIn);

        public static void SetOut(TextWriter newOut) => Console.SetOut(newOut);

        public static void SetWindowPosition(int left, int top) => Console.SetWindowPosition(left, top);

        public static void SetWindowSize(int width, int height) => Console.SetWindowSize(width, height);

        public static void Write(bool value) => Console.Write(value);

        public static void Write(char value) => Console.Write(value);

        public static void Write(char[] buffer) => Console.Write(buffer);

        public static void Write(char[] buffer, int index, int count) => Console.Write(buffer, index, count);

        public static void Write(decimal value) => Console.Write(value);

        public static void Write(double value) => Console.Write(value);

        public static void Write(float value) => Console.Write(value);

        public static void Write(int value) => Console.Write(value);

        public static void Write(long value) => Console.Write(value);

        public static void Write(object value) => Console.Write(value);

        public static void Write(uint value) => Console.Write(value);

        public static void Write(ulong value) => Console.Write(value);

        public static void Write(string value) => Console.Write(value);

        public static void Write(string format, object arg0) => Console.Write(format, arg0);

        public static void Write(string format, object arg0, object arg1) => Console.Write(format, arg0, arg1);

        public static void Write(string format, object arg0, object arg1, object arg2) => Console.Write(format, arg0, arg1, arg2);

        public static void Write(string format, params object[] arg) => Console.Write(format, arg);

        public static void WriteLine() => Console.WriteLine();

        public static void WriteLine(bool value) => Console.WriteLine(value);

        public static void WriteLine(char value) => Console.WriteLine(value);

        public static void WriteLine(char[] buffer) => Console.WriteLine(buffer);

        public static void WriteLine(char[] buffer, int index, int count) => Console.WriteLine(buffer, index, count);

        public static void WriteLine(decimal value) => Console.WriteLine(value);

        public static void WriteLine(double value) => Console.WriteLine(value);

        public static void WriteLine(float value) => Console.WriteLine(value);

        public static void WriteLine(int value) => Console.WriteLine(value);

        public static void WriteLine(long value) => Console.WriteLine(value);

        public static void WriteLine(object value) => Console.WriteLine(value);

        public static void WriteLine(uint value) => Console.WriteLine(value);

        public static void WriteLine(ulong value) => Console.WriteLine(value);

        public static void WriteLine(string value) => Console.WriteLine(value);

        public static void WriteLine(string format, object arg0) => Console.WriteLine(format, arg0);

        public static void WriteLine(string format, object arg0, object arg1) => Console.WriteLine(format, arg0, arg1);

        public static void WriteLine(string format, object arg0, object arg1, object arg2) => Console.WriteLine(format, arg0, arg1, arg2);

        public static void WriteLine(string format, params object[] arg) => Console.WriteLine(format, arg);

        public static event System.ConsoleCancelEventHandler CancelKeyPress {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }
    }
}
