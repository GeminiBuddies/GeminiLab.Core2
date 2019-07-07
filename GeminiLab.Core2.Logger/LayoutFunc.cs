using System;

namespace GeminiLab.Core2.Logger {
    public delegate string LayoutFunc(int level, string category, DateTime time, string content);
}