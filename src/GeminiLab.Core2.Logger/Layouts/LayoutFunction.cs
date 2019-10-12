using System;

namespace GeminiLab.Core2.Logger.Layouts {
    // it would be unnecessary if C# had anonymous inner class
    public delegate string LayoutFunction(int level, string category, DateTime time, string content);
}
