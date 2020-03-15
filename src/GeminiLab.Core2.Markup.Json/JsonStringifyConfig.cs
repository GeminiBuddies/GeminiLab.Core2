using System;

namespace GeminiLab.Core2.Markup.Json {
    [Flags]
    public enum JsonStringifyOption {
        None = 0,
        Inline = 1,
        Compact = 2,
        AsciiOnly = 4,
    }
}
