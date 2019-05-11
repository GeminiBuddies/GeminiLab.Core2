using Xunit;

using GeminiLab.Core2.Random;
using GeminiLab.Core2.Random.Sugar;

namespace XUnitTester.GeminiLab_Core2_Random {
    public class RNGExtensionTest {
        [Fact]
        public void NextDoubleTest() {
            var mt = new Mt19937X64(0x132df31b32c56a54ul);

            Assert.InRange(mt.NextDouble(), 0.0, 1.0);
            Assert.InRange(mt.NextDouble(), 0.0, 1.0);

            var mt32 = new Mt19937S(0x321a45d4);

            Assert.InRange(mt32.NextDouble(), 0.0, 1.0);
            Assert.InRange(mt32.NextDouble(), 0.0, 1.0);
        }

        [Fact]
        public void NextRangedTest() {
            var mt32 = new Mt19937S(0x5d54615c);

            Assert.InRange(mt32.Next(128), 0, 127);
            Assert.InRange(mt32.Next(128, 256), 128, 255);
        }

        [Fact]
        public void FillTest() {
            const int len = 262144;
            int[] count = new int[256];
            byte[] buffer = new byte[len];

            DefaultRNG.Instance.Fill(buffer);

            for (int i = 0; i < len; ++i) ++count[buffer[i]];
            for (int i = 0; i < 256; ++i) Assert.InRange(count[i] * 256.0 / len, 0.9, 1.1);
        }

        [Fact]
        public void BytesGenerateTest() {
            const int len = 262144;
            int[] count = new int[256];
            byte[] buffer = DefaultRNG.Instance.NextBytes(len);

            for (int i = 0; i < len; ++i) ++count[buffer[i]];
            for (int i = 0; i < 256; ++i) Assert.InRange(count[i] * 256.0 / len, 0.85, 1.15);
        }

        [Fact]
        public void ShuffleTest() {
            const int len = 262144;
            int[] v = new int[len];
            bool[] exists = new bool[len];

            for (int i = 0; i < len; ++i) v[i] = i;

            v.Shuffle();

            for (int i = 0; i < len; ++i) exists[v[i]] = true;
            for (int i = 0; i < len; ++i) Assert.True(exists[i]);
        }
    }
}
