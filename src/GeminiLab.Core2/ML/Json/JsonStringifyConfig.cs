using System;

namespace GeminiLab.Core2.ML.Json {
    [Flags]
    public enum JsonStringifyConfig {
        Default = 0,
        Compact = 1,
        ExpandObjects = 2,
        AsciiOnly = 4,

        Minimized = Compact,
        SingleLine = Default,
        Prettified = ExpandObjects,
        Network = Compact | AsciiOnly
    }

    internal static class JsonStringifyConfigExtension {
        internal static bool Contains(this JsonStringifyConfig conf, JsonStringifyConfig b) {
            return (conf & b) == b;
        }
    }
}
