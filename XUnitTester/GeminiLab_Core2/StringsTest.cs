using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GeminiLab.Core2;
using GeminiLab.Core2.Random;
using GeminiLab.Core2.Yielder;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTester.GeminiLab_Core2 {
    public class StringsTest {
        [Fact]
        public static void JoinTest() {
            var c = Strings.DigitAndLetter.MakeChooser();
            var ss = 16384.Times(() => DefaultRNG.I32.Next(1, 16)).Select(len => c.Take(len).ToList().Join()).ToList();

            Assert.Equal(string.Join("", ss), ss.Join());
            Assert.Equal(string.Join(",", ss), ss.JoinBy(","));
            Assert.Equal("-aloha-".Join(ss), ss.JoinBy("-aloha-"));

            var cc = 131072.Times(c.Next).ToList();
            Assert.Equal(string.Join("/", cc), cc.JoinBy("/"));
        }

        [Fact]
        public static void EncodeDecodeTest() {
            var str = Yielder.Repeat(() => (char) DefaultRNG.I32.Next(32, 0xd800)).Take(1048576).ToList().Join();
            var encoded = new UTF8Encoding(false).GetBytes(str);

            Assert.Equal(encoded, str.Encode());
            Assert.Equal(encoded, str.Encode(new UTF8Encoding(false)));
            Assert.Equal(encoded, str.Encode("utf-8"));

            Assert.Equal(str, encoded.Decode());
            Assert.Equal(str, encoded.Decode(Encoding.UTF8));
            Assert.Equal(str, encoded.Decode("utf-8"));
        }
    }
}
