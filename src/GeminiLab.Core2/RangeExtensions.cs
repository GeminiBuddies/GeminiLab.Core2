namespace GeminiLab.Core2 {
    public static class RangeExtensions {
        public static Range To(this int from, int to) => new Range(from, to);
        public static Range To(this int from, int to, int step) => new Range(from, to, step);
    }
}
