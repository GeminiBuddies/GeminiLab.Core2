using System;
using System.Text;

namespace GeminiLab.Core2.Base64 {
    public static class Base64 {
        public const string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        private static readonly byte[] DecodeTable = {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x3E, 0xFF, 0xFF, 0xFF, 0x3F,
            0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0xFF,
            0xFF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E,
            0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
            0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        };

        public static string ToBase64(this byte[] source) {
            var tail = source.Length % 3;
            var groups = source.Length / 3;
            var outputLen = groups * 4 + (tail > 0 ? 4 : 0);

            var sb = new StringBuilder(outputLen);

            for (int i = 0; i < groups; ++i) {
                int val = source[i * 3] << 16 | source[i * 3 + 1] << 8 | source[i * 3 + 2];

                sb.Append(Base64Chars[(val >> 18)]);
                sb.Append(Base64Chars[(val >> 12) & 0x3F]);
                sb.Append(Base64Chars[(val >> 6) & 0x3F]);
                sb.Append(Base64Chars[val & 0x3F]);
            }

            if (tail == 1) {
                int val = source[source.Length - 1];
                sb.Append(Base64Chars[val >> 2]);
                sb.Append(Base64Chars[(val & 0x03) << 4]);
                sb.Append("==");
            } else if (tail == 2) {
                int val = source[source.Length - 2] << 8 | source[source.Length - 1];
                sb.Append(Base64Chars[val >> 10]);
                sb.Append(Base64Chars[(val >> 4) & 0x3F]);
                sb.Append(Base64Chars[(val & 0x0F) << 2]);
                sb.Append("=");
            }

            return sb.ToString();
        }

        public static string ToBase64(this string source) => source.ToBase64(Encoding.UTF8);

        public static string ToBase64(this string source, Encoding encoding) => encoding.GetBytes(source).ToBase64();

        public static byte[] AsBase64(this string source) {
            int len = source.Length;

            if (len % 4 != 0) throw new ArgumentException(nameof(source));
            if (len == 0) return new byte[0];

            int sp = source[len - 1] == '=' ? (source[len - 2] == '=' ? 2 : 1) : 0;
            int groups = len / 4;
            int outputLen = groups * 3 - sp;

            byte[] rv = new byte[outputLen];

            for (int i = 0; i < groups; ++i) {
                int val = 0;
                for (int j = 0; j < 4; ++j) {
                    int chr = source[i * 4 + j];
                    if (chr >= 0x80 || chr < 0 || DecodeTable[chr] >= 0x40) throw new ArgumentException(nameof(source));

                    val = (val << 6) | DecodeTable[chr];
                }

                if (i == groups - 1) {
                    byte[] v = { (byte)(val >> 16), (byte)((val >> 8) & 0xFF), (byte)(val & 0xFF) };

                    for (int j = 0; j < 3; ++j) {
                        if (j + sp >= 3) {
                            if (v[j] != 0) throw new ArgumentException(nameof(source));
                        } else {
                            rv[i * 3 + j] = v[j];
                        }
                    }
                } else {
                    rv[i * 3] = (byte)(val >> 16);
                    rv[i * 3 + 1] = (byte)((val >> 8) & 0xFF);
                    rv[i * 3 + 2] = (byte)(val & 0xFF);
                }
            }

            return rv;
        }

        public static string DecodeBase64(this string source) => source.DecodeBase64(Encoding.UTF8);

        public static string DecodeBase64(this string source, Encoding encoding) => encoding.GetString(source.AsBase64());
    }
}
