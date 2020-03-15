using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GeminiLab.Core2 {
    [ExcludeFromCodeCoverage]
    public static class Chars {
        public static bool IsLatin1(this char ch) => (uint)ch <= '\x00ff';
        public static bool IsAscii(this char ch) => (uint)ch <= '\x007f';
        public static bool IsInRange(this char ch, char min, char max) => (uint)(ch - min) <= (uint)(max - min);

        public static bool IsNumber(this char ch) => char.IsNumber(ch);
        public static bool IsDigit(this char ch) => char.IsDigit(ch);
        public static bool IsLetter(this char ch) => char.IsLetter(ch);
        public static bool IsLetterOrDigit(this char ch) => char.IsLetterOrDigit(ch);
        public static bool IsUpper(this char ch) => char.IsUpper(ch);
        public static bool IsLower(this char ch) => char.IsLower(ch);

        public static bool IsControl(this char ch) => char.IsControl(ch);
        public static bool IsHighSurrogate(this char ch) => char.IsHighSurrogate(ch);
        public static bool IsLowSurrogate(this char ch) => char.IsLowSurrogate(ch);
        public static bool IsSurrogate(this char ch) => char.IsSurrogate(ch);
        public static bool IsPunctuation(this char ch) => char.IsPunctuation(ch);
        public static bool IsSeparator(this char ch) => char.IsSeparator(ch);
        public static bool IsSymbol(this char ch) => char.IsSymbol(ch);
        public static bool IsWhitespace(this char ch) => char.IsWhiteSpace(ch);

        public static bool IsOctalDigit(this char ch) => IsInRange(ch, '0', '7');
        public static bool IsDecimalDigit(this char ch) => IsInRange(ch, '0', '9');
        public static bool IsHexadecimalDigit(this char ch) => IsInRange(ch, '0', '9') || IsInRange((char)(ch | 0x20), 'a', 'f');
        public static bool IsHexadecimalDigitUpper(this char ch) => IsInRange(ch, '0', '9') || IsInRange(ch, 'A', 'F');
        public static bool IsHexadecimalDigitLower(this char ch) => IsInRange(ch, '0', '9') || IsInRange(ch, 'a', 'f');
    }
}
