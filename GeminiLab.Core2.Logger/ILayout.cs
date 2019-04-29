using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger {
    public interface ILayout {
        string Format(int level, string category, string content);
    }
}
