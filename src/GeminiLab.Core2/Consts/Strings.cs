namespace GeminiLab.Core2.Consts {
    public static class Strings {
        public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string Letter = Lowercase + Uppercase;

        public const string DigitOct = "01234567";
        public const string Digit = "0123456789";
        public const string DigitHexLower = "0123456789abcdef";
        public const string DigitHexUpper = "0123456789ABCDEF";
        public const string DigitHex = "0123456789abcdefABCDEF";

        public const string DigitAndLowercase = Digit + Lowercase;
        public const string DigitAndUppercase = Digit + Uppercase;
        public const string DigitAndLetter = Digit + Letter;
    }
}
