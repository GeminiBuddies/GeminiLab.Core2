using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace GeminiLab.Core2.Logger {
    public sealed class LoggerContext : IDisposable {
        private readonly Dictionary<string, LoggerCategory> _categories = new Dictionary<string, LoggerCategory>();
        private readonly Dictionary<string, IAppender> _appenders = new Dictionary<string, IAppender>();

        public Logger GetLogger(string category) {
            if (!_categories.TryGetValue(category, out var categoryItem)) return null;
            return new Logger(categoryItem);
        }

        public void AddCategory(string category) {
            if (!_categories.ContainsKey(category)) _categories.Add(category, new LoggerCategory(category));
        }

        public void AddAppender(string name, IAppender appender) {
            _appenders.Add(name, appender);
        }

        public bool Connect(string category, string appender, params IFilter[] filter) {
            if (!_categories.TryGetValue(category, out var categoryItem)) return false;
            if (!_appenders.TryGetValue(appender, out var appenderItem)) return false;

            categoryItem.AddConnection(appenderItem, filter);
            return true;
        }

        private void disposeAppenders() {
            foreach (var pair in _appenders) {
                var appender = pair.Value;

                if (appender is IDisposable disposable) disposable.Dispose(); 
            }
        }

        public void Dispose() {
            disposeAppenders();
            GC.SuppressFinalize(this);
        }

        ~LoggerContext() {
            disposeAppenders();
        }
    }
}
