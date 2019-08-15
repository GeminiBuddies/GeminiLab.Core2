namespace GeminiLab.Core2.Stream {
    public interface ISeekableStream : IStream {
        long Seek(long offset, SeekOrigin origin);
        long Length { get; set; }
        long Position { get; set; }
    }
}
