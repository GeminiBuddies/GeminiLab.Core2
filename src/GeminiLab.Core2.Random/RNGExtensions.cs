using System;
using GeminiLab.Core2.Random.RNG;

namespace GeminiLab.Core2.Random {
    // accept two types of RNGs as basic numeric RNGs
    // - IRNG<int>
    // - IRNG<ulong>
    public static class RNGExtensions {
        public static double NextDouble(this IRNG<int> rng) => unchecked((uint)rng.Next()) * (1.0 / uint.MaxValue);
        public static double NextDouble(this IRNG<ulong> rng) => rng.Next() * (1.0 / ulong.MaxValue);

        public static int Next(this IRNG<int> rng, int min, int max) {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(min));

            return (int)(min + Next(rng, (uint)(max - min)));
        }

        public static int Next(this IRNG<int> rng, int max) => rng.Next(0, max);

        public static uint Next(this IRNG<int> rng, uint min, uint max) {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(min));

            return min + Next(rng, max - min);
        }

        public static uint Next(this IRNG<int> rng, uint max) {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));

            unchecked {
                var threshold = 0 - (0 - max) % max;

                uint rn;
                do {
                    rn = (uint)rng.Next();
                } while (rn > threshold && threshold > 0);

                return rn % max;
            }
        }

        public static ulong Next(this IRNG<ulong> rng, ulong min, ulong max) {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(min));

            return min + Next(rng, max - min);
        }

        public static ulong Next(this IRNG<ulong> rng, ulong max) {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));

            unchecked {
                var threshold = 0 - (0 - max) % max;

                uint rn;
                do {
                    rn = (uint)rng.Next();
                } while (rn > threshold);

                return rn % max;
            }
        }

        public static void Fill(this IRNG<int> rng, byte[] buffer) => Fill(rng, buffer ?? throw new ArgumentNullException(nameof(buffer)), 0, buffer.Length);

        // 4x faster than old one
        public static void Fill(this IRNG<int> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0 || start >= len) throw new ArgumentOutOfRangeException(nameof(start));
            if (count < 0 || start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;

            int ptr = start;
            while ((ptr & 0x03) != 0 && ptr < end) {
                buffer[ptr++] = (byte)(rng.Next() & 0xFF);
            }

            unsafe {
                fixed (byte* bptr = buffer) {
                    while ((ptr + 4) <= end) {
                        *(int*) (bptr + ptr) = rng.Next();
                        ptr += 4;
                    }
                }
            }

            while (ptr < end) {
                buffer[ptr++] = (byte)(rng.Next() & 0xFF);
            }
        }

        public static void Fill(this IRNG<ulong> rng, byte[] buffer) => Fill(rng, buffer ?? throw new ArgumentNullException(nameof(buffer)), 0, buffer.Length);

        public static void Fill(this IRNG<ulong> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0 || start >= len) throw new ArgumentOutOfRangeException(nameof(start));
            if (count < 0 || start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;

            int ptr = start;
            while ((ptr & 0x07) != 0 && ptr < end) {
                buffer[ptr++] = (byte)(rng.Next() & 0xFF);
            }

            unsafe {
                fixed (byte* bptr = buffer) {
                    while ((ptr + 8) <= end) {
                        *(ulong*)(bptr + ptr) = rng.Next();
                        ptr += 8;
                    }
                }
            }

            while (ptr < end) {
                buffer[ptr++] = (byte)(rng.Next() & 0xFF);
            }
        }

        public static byte[] NextBytes(this IRNG<int> rng, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return new byte[0];

            var rv = new byte[count];
            rng.Fill(rv);
            return rv;
        }

        public static byte[] NextBytes(this IRNG<ulong> rng, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return new byte[0];

            var rv = new byte[count];
            rng.Fill(rv);
            return rv;
        }

        public static IRNG<ulong> AsU64RNG(this IRNG<int> rng) => new I32ToU64RNG(rng);

        public static void Shuffle<T>(this T[] array) {
            ShuffleBy(array, DefaultRNG.I32);
        }

        public static void ShuffleBy<T>(this T[] array, IRNG<int> rng) {
            int l = array.Length;

            for (int i = 0; i < l - 1; ++i) {
                var j = rng.Next(i, l);

                if (i == j) continue;
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
        }
    }
}
