using System;

namespace GeminiLab.Core2.Logger {
    public delegate  void AppenderFunc(int level, string category, DateTime time, string content);
}
