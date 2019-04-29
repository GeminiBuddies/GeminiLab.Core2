namespace GeminiLab.Core2.Logger {
    public static class Filters {
        public static Filter DenyFilter => (_, __, ___) => false;

        public static Filter Threshold(int levelThreshold) => (level, _, __) => level >= levelThreshold;
    }
}
