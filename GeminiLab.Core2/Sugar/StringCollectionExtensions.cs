using System.Collections.Generic;

namespace GeminiLab.Core2.Sugar {
    public static class StringCollectionExtensions {
        public static string JoinBy(this IEnumerable<string> value, string separator) => string.Join(separator, value);
        public static string JoinBy(this IEnumerable<char> value, string separator) => string.Join(separator, value);

        public static string Join(this IEnumerable<string> value) => string.Join("", value);
        public static string Join(this IEnumerable<char> value) => string.Join("", value);

        public static string Join(this string separator, IEnumerable<string> value) => string.Join(separator, value);
    }
}
