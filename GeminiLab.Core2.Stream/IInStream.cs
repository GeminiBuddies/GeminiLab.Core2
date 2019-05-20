namespace GeminiLab.Core2.Stream {
    public interface IInStream : IStream {
        int Read(byte[] buffer, int offset, int count);
    }
}
