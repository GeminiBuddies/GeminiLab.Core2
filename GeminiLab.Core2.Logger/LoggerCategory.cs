using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Logger {
    internal class LoggerCategory {
        private readonly List<(IEnumerable<IFilter> filters, IAppender appender)> _connections = new List<(IEnumerable<IFilter> filters, IAppender appender)>();
        private readonly string _name;
        public LoggerCategory(string name) {
            _name = name;
        }

        public void AddConnection(IAppender appender, params IFilter[] filters) {
            _connections.Add((filters, appender));
        }

        public void Invoke(int level, string content) {
            foreach (var (filters, appender) in _connections) {
                if (filters.All(filter => filter.Accept(level, content))) appender.Append(level, _name, content);
            }
        }
    }
}