namespace GeminiLab.Core2 {
    public static class Strings {
        public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string Letter = Lowercase + Uppercase;

        public const string Digit = "0123456789";
        public const string DigitHexOnlyLower = "abcdef";
        public const string DigitHexOnlyUpper = "ABCDEF";
        public const string DigitHexOnly = DigitHexOnlyLower + DigitHexOnlyUpper;
        public const string DigitHexLower = Digit + DigitHexOnlyLower;
        public const string DigitHexUpper = Digit + DigitHexOnlyUpper;
        public const string DigitHex = Digit + DigitHexOnlyLower + DigitHexOnlyUpper;

        public const string DigitAndLowercase = Digit + Lowercase;
        public const string DigitAndUppercase = Digit + Uppercase;
        public const string DigitAndLetter = Digit + Letter;
    }
}
