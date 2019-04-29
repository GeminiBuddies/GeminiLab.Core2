using System;
using System.Collections.Generic;
using System.Text;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger {
    public static class Layout {
        private static readonly DefaultLayout SharedDefaultLayout = new DefaultLayout();
        public static ILayout DefaultLayout => SharedDefaultLayout;

        private static readonly DefaultTimedLayout SharedDefaultTimedLayout = new DefaultTimedLayout();
        public static ILayout DefaultTimedLayout => SharedDefaultTimedLayout;

        private static readonly MinimumLayout SharedMinimumLayout = new MinimumLayout();
        public static ILayout MinimumLayout => SharedMinimumLayout;

        private static readonly ColorfulConsoleLayout SharedColorfulConsoleLayout = new ColorfulConsoleLayout();
        public static ILayout ColorfulConsoleLayout => SharedColorfulConsoleLayout;

        private static readonly ColorfulTimedConsoleLayout SharedColorfulTimedConsoleLayout = new ColorfulTimedConsoleLayout();
        public static ILayout ColorfulTimedConsoleLayout => SharedColorfulTimedConsoleLayout;
    }
}
