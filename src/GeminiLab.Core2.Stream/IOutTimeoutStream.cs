using System;

namespace GeminiLab.Core2.Stream {
    public interface IOutTimeoutStream : IOutStream {
        TimeSpan WriteTimeout { get; set; }
    }
}
