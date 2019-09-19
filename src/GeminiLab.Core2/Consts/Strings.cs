namespace GeminiLab.Core2.Consts {
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
    }
}
