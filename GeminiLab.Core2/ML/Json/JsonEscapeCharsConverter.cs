using System;
using System.Text;
using System.Linq;

namespace GeminiLab.Core2.ML.Json {
    internal static class JsonEscapeCharsConverter {
        internal static string Decode(string src) {
            int length = src.Length;
            var sourceArray = src.ToCharArray();
            var sb = new StringBuilder(length / 4); // not so likely to be shorter than this.

            for (int i = 0; i < length; ++i) {
                if (sourceArray[i] == '\\') {
                    if (i + 1 == length) throw new Exception(); // todo: write a new exception class

                    switch (sourceArray[i + 1]) {
                    case '\"':
                    case '\\':
                    case '/':
                        sb.Append(sourceArray[i + 1]);
                        break;
                    case 'b':
                        sb.Append('\b');
                        break;
                    case 'f':
                        sb.Append('\f');
                        break;
                    case 'n':
                        sb.Append('\n');
                        break;
                    case 'r':
                        sb.Append('\r');
                        break;
                    case 't':
                        sb.Append('\t');
                        break;
                    case 'u':
                        if (i + 5 >= length) throw new Exception();

                        int unicode = 0;
                        for (int it = i + 2; it < i + 6; ++it) {
                            if (!Strings.DigitHex.Contains(sourceArray[it])) throw new Exception();
                            unicode = unicode * 16 + (sourceArray[it] < '9' ? sourceArray[it] - '0' : (sourceArray[it] & 0xDF) - 'A' + 10);
                        }

                        sb.Append((char) unicode);
                        break;
                    default:
                        throw new Exception();
                    }

                    if (sourceArray[i + 1] == 'u') i += 5;
                    else i += 1;
                } else {
                    sb.Append(sourceArray[i]);
                }
            }

            return sb.ToString();
        }

        internal static string Encode(string source) {
            throw new NotImplementedException();
        }

        internal static string EncodeToAscii(string source) {
            throw new NotImplementedException();
        }
    }
}
