using System;

namespace GeminiLab.Core2.Logger {
    public interface IAppender {
        void Append(int level, string category, DateTime time, string content);
    }
}