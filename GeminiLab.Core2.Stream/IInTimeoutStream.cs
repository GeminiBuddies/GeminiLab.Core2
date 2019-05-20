using System;

namespace GeminiLab.Core2.Stream {
    public interface IInTimeoutStream : IInStream {
        TimeSpan ReadTimeout { get; set; }
    }
}
