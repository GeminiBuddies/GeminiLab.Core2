using System;
using System.Collections.Generic;
using GeminiLab.Core2.Logger;
using Xunit;

namespace XUnitTester.GeminiLab_Core2_Logger {
    internal class FalseAppender : IAppender {
        public Queue<(int Level, string Category, DateTime Time, string Content)> Messages { get; } = new Queue<(int, string, DateTime, string)>();

        public void Append(int level, string category, DateTime time, string content) =>
            Messages.Enqueue((level, category, time, content));

        public void Assert(int level, string category, string content) {
            Xunit.Assert.NotEmpty(Messages);

            var first = Messages.Dequeue();
            Xunit.Assert.Equal(level, first.Level);
            Xunit.Assert.Equal(category, first.Category);
            Xunit.Assert.Equal(content, first.Content);
        }

        public void AssertEnd() => Xunit.Assert.Empty(Messages);
    }

    public static class LoggerTest {
        [Fact]
        public static void NormalTest() {
            using var context = new LoggerContext();

            var app1 = new FalseAppender();
            var app2 = new FalseAppender();
            context.AddAppender("app1", app1);
            context.AddAppender("app2", app2);

            context.AddCategory("cat1");
            context.AddCategory("cat2");
            context.AddCategory("cat3");

            context.Connect("cat1", "app1");
            context.Connect("cat1", "app2", Filters.AcceptFilter, Filters.DenyFilter);
            context.Connect("cat2", "app2", Filters.AcceptFilter);
            context.Connect("cat2", "app1", Filters.Threshold(Logger.LevelWarn));
            context.Connect("cat3", "app1", Filters.Threshold(Logger.LevelWarn, Logger.LevelFatal));
            context.Connect("cat3", "app2", (level, category, content) => content.Length > 32);

            var log1 = context.GetLogger("cat1");
            var log2 = context.GetLogger("cat2");
            var log3 = context.GetLogger("cat3");

            Assert.Null(context.GetLogger("cat4"));

            log1.Trace("cat1:trace");
            log1.Debug("cat1:debug");
            log1.Info("cat1:info");
            log2.Debug("cat2:DEBUG");
            log2.Info("cat2:INFO");
            log2.Warn("cat2:WARN");
            log3.Info("cat3:info");
            log3.Warn("cat3:warn");
            log3.Error("cat3:7xError,7xError,7xError,7xError,7xError,7xError,7xError");
            log3.Fatal("cat3:7xFatal,7xFatal,7xFatal,7xFatal,7xFatal,7xFatal,7xFatal");
            log1.Log(4396, "level4396");

            app1.Assert(Logger.LevelTrace, "cat1", "cat1:trace");
            app1.Assert(Logger.LevelDebug, "cat1", "cat1:debug");
            app1.Assert(Logger.LevelInfo, "cat1", "cat1:info");
            app1.Assert(Logger.LevelWarn, "cat2", "cat2:WARN");
            app1.Assert(Logger.LevelWarn, "cat3", "cat3:warn");
            app1.Assert(Logger.LevelError, "cat3", "cat3:7xError,7xError,7xError,7xError,7xError,7xError,7xError");
            app1.Assert(4396, "cat1", "level4396");
            app1.AssertEnd();

            app2.Assert(Logger.LevelDebug, "cat2", "cat2:DEBUG");
            app2.Assert(Logger.LevelInfo, "cat2", "cat2:INFO");
            app2.Assert(Logger.LevelWarn, "cat2", "cat2:WARN");
            app2.Assert(Logger.LevelError, "cat3", "cat3:7xError,7xError,7xError,7xError,7xError,7xError,7xError");
            app2.Assert(Logger.LevelFatal, "cat3", "cat3:7xFatal,7xFatal,7xFatal,7xFatal,7xFatal,7xFatal,7xFatal");
            app1.AssertEnd();
        }
    }
}
