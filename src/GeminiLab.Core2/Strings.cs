using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2 {
    public static class Strings {
        public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string Letter = Lowercase + Uppercase;

        public const string DigitOctal = "01234567";
        public const string Digit = "0123456789";
        public const string DigitDecimal = Digit;
        public const string DigitHexadecimalLower = "0123456789abcdef";
        public const string DigitHexadecimalUpper = "0123456789ABCDEF";
        public const string DigitHexadecimal = "0123456789abcdefABCDEF";

        public const string DigitAndLowercase = Digit + Lowercase;
        public const string DigitAndUppercase = Digit + Uppercase;
        public const string DigitAndLetter = Digit + Letter;

        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false);

        public static byte[] Encode(this string s) => DefaultEncoding.GetBytes(s);
        public static byte[] Encode(this string s, Encoding encoding) => encoding.GetBytes(s);
        public static byte[] Encode(this string s, string encoding) => Encoding.GetEncoding(encoding).GetBytes(s);

        public static string Decode(this byte[] bytes) => DefaultEncoding.GetString(bytes);
        public static string Decode(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);
        public static string Decode(this byte[] bytes, string encoding) => Encoding.GetEncoding(encoding).GetString(bytes);

        public static string JoinBy(this IEnumerable<string> value, string separator) => string.Join(separator, value);
        public static string JoinBy(this IEnumerable<char> value, string separator) => string.Join(separator, value);

        public static string Join(this IEnumerable<string> value) => string.Concat(value);
        public static string Join(this IEnumerable<char> value) => string.Join("", value);

        public static string Join(this string separator, IEnumerable<string> value) => string.Join(separator, value);
    }
}
