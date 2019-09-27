using System;
using System.IO;
using System.Text;

namespace GeminiLab.Core2.IO {
    /// <summary>DO NOT use async functions inherited from TextWriter.</summary>
    public class IndentedWriter : TextWriter {
        private readonly TextWriter _internalWriter;

        public IndentedWriter(TextWriter internalWriter): this(internalWriter, null) {}

        public IndentedWriter(TextWriter internalWriter, IFormatProvider? formatProvider): base(formatProvider) {
            _internalWriter = internalWriter ?? throw new ArgumentNullException(nameof(internalWriter));
            _indent = 0;
            _indentWritten = false;
        }

        public virtual string IndentString { get; set; } = "    ";

        private int _indent;

        public virtual int Indent {
            get => _indent;
            set => _indent = value < 0 ? 0 : value;
        }

        public override string NewLine {
            get => base.NewLine;
            set => base.NewLine = _internalWriter.NewLine = value;
        }

        public void IncreaseIndent() => IncreaseIndent(1);
        public virtual void IncreaseIndent(int value) => Indent += value;
        public void DecreaseIndent() => DecreaseIndent(1);
        public virtual void DecreaseIndent(int value) => Indent -= value;

        private bool _indentWritten;

        protected virtual void EnsureIndent() {
            if (_indentWritten) return;

            for (int i = 0; i < Indent; ++i) _internalWriter.Write(IndentString);
            _indentWritten = true;
        }

        public override void Write(char c) {
            EnsureIndent();
            _internalWriter.Write(c);
        }

        public override void Write(char[] buffer, int index, int count) {
            EnsureIndent();
            _internalWriter.Write(buffer, index, count);
        }

        public override void WriteLine(string value) {
            EnsureIndent();
            _internalWriter.Write(value);
            WriteLine();
        }

        public override void WriteLine() {
            _internalWriter.WriteLine();
            _indentWritten = false;
        }
        
        public override Encoding Encoding => _internalWriter.Encoding;

        protected bool disposed = false;
        protected override void Dispose(bool disposing) {
            if (disposed) return;

            if (disposing) {
                _internalWriter.Dispose();
            }

            disposed = true;
        }

        public override void Flush() {
            _internalWriter.Flush();
        }
    }
}
