using System;

namespace GeminiLab.Core2.Logger.Appenders {
    // it would be unnecessary if C# had anonymous inner class
    public delegate void AppenderFunction(int level, string category, DateTime time, string content);
}
