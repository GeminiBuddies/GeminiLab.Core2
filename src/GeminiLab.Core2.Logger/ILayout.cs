using System;

namespace GeminiLab.Core2.Logger {
    public interface ILayout {
        string Format(int level, string category, DateTime time, string content);
    }
}
