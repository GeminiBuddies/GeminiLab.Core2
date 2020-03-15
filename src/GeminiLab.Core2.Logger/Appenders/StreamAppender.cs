using System;
using System.IO;
using System.Text;
using GeminiLab.Core2.Logger.Layouts;

namespace GeminiLab.Core2.Logger.Appenders {
    public class StreamAppender : IAppender, IDisposable {
        private ILayout _layout;
        private StreamWriter _writer;

        public StreamAppender(Stream stream) : this(stream, DefaultLayout.Default, new UTF8Encoding(false)) { }

        public StreamAppender(Stream stream, ILayout layout) : this(stream, layout, new UTF8Encoding(false)) { }

        public StreamAppender(Stream stream, Encoding encoding) : this(stream, DefaultLayout.Default, encoding) { }

        public StreamAppender(Stream stream, ILayout layout, Encoding encoding) {
            _layout = layout;
            _writer = new StreamWriter(stream, encoding);
        }

        public void Append(int level, string category, DateTime time, string content) {
            _writer.WriteLine(_layout.Format(level, category, time, content));
        }

        private bool _disposed = false;
        private void dispose(bool disposing) {
            if (_disposed) return;

            try {
                if (disposing) {
                    _writer?.Dispose();
                }
            } finally {
                _layout = null!;
                _writer = null!;
            }

            _disposed = true;
        }

        public void Dispose() {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StreamAppender() {
            dispose(false);
        }
    }
}
