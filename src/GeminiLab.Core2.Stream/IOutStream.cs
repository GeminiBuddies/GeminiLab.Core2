namespace GeminiLab.Core2.Stream {
    public interface IOutStream : IStream {
        int Write(byte[] buffer, int offset, int count);
        void Flush();
    }
}
