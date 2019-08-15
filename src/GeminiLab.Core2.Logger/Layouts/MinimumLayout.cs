using System;

namespace GeminiLab.Core2.Logger.Layouts {
    public class MinimumLayout : ILayout {
        public string Format(int level, string category, DateTime time, string content) => content;

        public static MinimumLayout Default { get; } = new MinimumLayout();
    }
}
