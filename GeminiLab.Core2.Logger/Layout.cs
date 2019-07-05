using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Text;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger {
    public static class Layout {
        public static ILayout DefaultLayout { get; } = new DefaultLayout();
        public static ILayout DefaultTimedLayout { get; } = new DefaultTimedLayout();
        public static ILayout MinimumLayout { get; } = new MinimumLayout();
        public static ILayout DefaultColorfulConsoleLayout { get; } = new ColorfulConsoleLayout();
        public static ILayout DefaultTimedColorfulConsoleLayout { get; } = new ColorfulTimedConsoleLayout();

        public static ILayout ColorfulTimedConsoleLayout(string format) => new ColorfulTimedConsoleLayout(format);

        public static ILayout ColorfulTimedConsoleLayout(CultureInfo culture) => new ColorfulTimedConsoleLayout(culture);
    }
}
