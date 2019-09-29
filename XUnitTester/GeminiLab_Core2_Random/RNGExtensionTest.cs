using System;
using Xunit;

using GeminiLab.Core2.Random;
using GeminiLab.Core2.Random.RNG;

namespace XUnitTester.GeminiLab_Core2_Random {
    public static class RNGExtensionTest {
        [Fact]
        public static void NextDoubleTest() {
            const int length = 16384;

            for (int i = 0; i < length; ++i) Assert.InRange(DefaultRNG.U64.NextDouble(), 0.0, 1.0);
            for (int i = 0; i < length; ++i) Assert.InRange(DefaultRNG.I32.NextDouble(), 0.0, 1.0);
        }

        [Fact]
        public static void NextRangedTest() {
            const int length = 16384;

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.Next();
                var mi = DefaultRNG.Next();

                if (ma <= mi) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Next(mi, ma));
                else Assert.InRange(DefaultRNG.I32.Next(mi, ma), mi, ma - 1);
            }

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.Next();

                if (ma <= 0) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Next(0, ma));
                else Assert.InRange(DefaultRNG.I32.Next(0, ma), 0, ma - 1);
            }

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.NextU32();
                var mi = DefaultRNG.NextU32();

                if (ma <= mi) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Next(mi, ma));
                else Assert.InRange(DefaultRNG.I32.Next(mi, ma), mi, ma - 1);
            }

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.NextU32();

                if (ma <= 0) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Next(0u, ma));
                else Assert.InRange(DefaultRNG.I32.Next(0u, ma), 0u, ma - 1);
            }

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.NextU64();
                var mi = DefaultRNG.NextU64();

                if (ma <= mi) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.U64.Next(mi, ma));
                else Assert.InRange(DefaultRNG.U64.Next(mi, ma), mi, ma - 1);
            }

            for (int i = 0; i < length; ++i) {
                var ma = DefaultRNG.NextU64();

                if (ma <= 0) Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.U64.Next(0u, ma));
                else Assert.InRange(DefaultRNG.U64.Next(0u, ma), 0u, ma - 1);
            }
        }

        private static void TestRandomByteArray(ReadOnlySpan<byte> arr) {
            int[] count = new int[256];
            var len = arr.Length;

            for (int i = 0; i < len; ++i) ++count[arr[i]];
            for (int i = 0; i < 256; ++i) Assert.InRange(count[i] * 256.0 / len, 0.8, 1.2);
        }

        [Fact]
        public static void FillTest() {
            Assert.Throws<ArgumentNullException>(() => DefaultRNG.I32.Fill(null));
            Assert.Throws<ArgumentNullException>(() => DefaultRNG.U64.Fill(null));
            Assert.Throws<ArgumentNullException>(() => DefaultRNG.I32.Fill(null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => DefaultRNG.U64.Fill(null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => RNGExtensions.Fill((IRNG<int>)null, new byte[0]));
            Assert.Throws<ArgumentNullException>(() => RNGExtensions.Fill((IRNG<ulong>)null, new byte[0]));

            const int length = 2048;
            const int fillTimes = 4096;
            var bufL = new byte[fillTimes * length];
            var buffer = new byte[length];
            var another = new byte[length];
            var shouldFilledCount = 0;
            var actuallyFilledCount = 0;

            DefaultRNG.I32.Fill(bufL);
            TestRandomByteArray(bufL.AsSpan());

            DefaultRNG.U64.Fill(bufL);
            TestRandomByteArray(bufL.AsSpan());

            for (int i = 0; i < fillTimes; ++i) {
                Array.Copy(buffer, another, length);

                var s = DefaultRNG.I32.Next(-length / 16, length + length / 16);
                var l = DefaultRNG.I32.Next(-length / 16, length);

                if (s >= length || s < 0 || l < 0 || s + l > length) {
                    Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Fill(buffer, s, l));
                } else {
                    shouldFilledCount += l;
                    DefaultRNG.I32.Fill(buffer, s, l);

                    for (int p = 0; p < length; ++p) {
                        if (p >= s && p < s + l) {
                            if (another[p] != buffer[p]) ++actuallyFilledCount;
                        } else {
                            Assert.Equal(another[p], buffer[p]);
                        }
                    }
                }

                Array.Copy(buffer, another, length);

                s = DefaultRNG.I32.Next(0, length + length / 16);
                l = DefaultRNG.I32.Next(-length / 16, length);

                if (s >= length || s < 0 || l < 0 || s + l > length) {
                    Assert.Throws<ArgumentOutOfRangeException>(() => DefaultRNG.I32.Fill(buffer, s, l));
                } else {
                    shouldFilledCount += l;
                    DefaultRNG.U64.Fill(buffer, s, l);

                    for (int p = 0; p < length; ++p) {
                        if (p >= s && p < s + l) {
                            if (another[p] != buffer[p]) ++actuallyFilledCount;
                        } else {
                            Assert.Equal(another[p], buffer[p]);
                        }
                    }
                }
            }

            Assert.True((double)actuallyFilledCount / shouldFilledCount > 0.98);
        }

        [Fact]
        public static void NextBytesTest() {
            TestRandomByteArray(DefaultRNG.I32.NextBytes(1048576).AsSpan());
            TestRandomByteArray(DefaultRNG.U64.NextBytes(1048576).AsSpan());
            Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomByteArray(DefaultRNG.I32.NextBytes(-1).AsSpan()));
            Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomByteArray(DefaultRNG.U64.NextBytes(-2).AsSpan()));
        }

        [Fact]
        public static void BytesGenerateTest() {
            const int len = 262144;
            int[] count = new int[256];
            byte[] buffer = DefaultRNG.I32.NextBytes(len);

            for (int i = 0; i < len; ++i) ++count[buffer[i]];
            for (int i = 0; i < 256; ++i) Assert.InRange(count[i] * 256.0 / len, 0.8, 1.2);
        }

        [Fact]
        public static void ShuffleTest() {
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
