namespace GeminiLab.Core2.Logger {
    public static class Filters {
        public static Filter AcceptFilter => (_, __, ___) => true;
        public static Filter DenyFilter => (_, __, ___) => false;

        public static Filter Threshold(int min) => (level, _, __) => level >= min;

        public static Filter Threshold(int min, int max) => (level, _, __) => level >= min && level < max;
    }
}
