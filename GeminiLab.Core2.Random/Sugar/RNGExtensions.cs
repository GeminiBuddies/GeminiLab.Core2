using System;

namespace GeminiLab.Core2.Random.Sugar {
    public static class RandomExtensions {
        public static double NextDouble(this IRNG<int> rng) => unchecked((uint)rng.Next()) * (1.0 / uint.MaxValue);
        public static double NextDouble(this IRNG<ulong> rng) => rng.Next() * (1.0 / ulong.MaxValue);

        public static int Next(this IRNG<int> rng, int min, int max) {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(min));

            return min + Next(rng, max - min);
        }

        public static int Next(this IRNG<int> rng, int max) {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));

            // how to do:
            //  0x100000000ul % max = threshold
            //  while random-number-get >= 0x100000000ul - threshold: gen-new-random-number

            var threshold = 0x1_0000_0000 - 0x1_0000_0000 % max;
            uint rn;
            do {
                rn = unchecked((uint)rng.Next());
            } while (rn > threshold);

            return unchecked((int)(rn % max));
        }

        public static void Fill(this IRNG<int> rng, byte[] buffer) => Fill(rng, buffer ?? throw new ArgumentNullException(nameof(buffer)), 0, buffer.Length);

        public static void Fill(this IRNG<int> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0 || start >= len) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;
            for (int i = start; i < end; ++i) buffer[i] = (byte)(rng.Next() & 0xFF);
        }

        public static byte[] NextBytes(this IRNG<int> rng, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return new byte[0];

            var rv = new byte[count];
            rng.Fill(rv);
            return rv;
        }

        public static void Shuffle<T>(this T[] array) {
            ShuffleBy(array, DefaultRNG.Instance);
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
