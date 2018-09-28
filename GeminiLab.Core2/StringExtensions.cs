using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2 {
    public static class StringExtensions {
        public static byte[] Encode(this string s) => Encoding.UTF8.GetBytes(s);
        public static byte[] Encode(this string s, Encoding encoding) => encoding.GetBytes(s);
        public static byte[] Encode(this string s, string encoding) => Encoding.GetEncoding(encoding).GetBytes(s);

        public static string Decode(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public static string Decode(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);
        public static string Decode(this byte[] bytes, string encoding) => Encoding.GetEncoding(encoding).GetString(bytes);
    }
    
    public static class EnumerableOfString {
        public static string JoinBy(this IEnumerable<string> value, string separator) => string.Join(separator, value);
        public static string JoinBy(this IEnumerable<char> value, string separator) => string.Join(separator, value);

        public static string Join(this IEnumerable<string> value) => string.Join("", value);
        public static string Join(this IEnumerable<char> value) => string.Join("", value);

        public static string Join(this string separator, IEnumerable<string> value) => string.Join(separator, value);
    }
}
