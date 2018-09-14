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
}
