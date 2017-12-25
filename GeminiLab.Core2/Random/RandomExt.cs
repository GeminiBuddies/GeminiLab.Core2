using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using SR = System.Random;

namespace GeminiLab.Core2.Random {
    public static class RandomExtensions {
        private static readonly SR _defaultSr = new SR(
            Environment.CurrentManagedThreadId ^ (Environment.TickCount << 16) ^ DateTime.UtcNow.ToString("fffssmmHHyyyyMMdd").GetHashCode()
        );

        public static double NextDouble(this IRNG<uint> rng) => rng.Next() * (1.0 / uint.MaxValue);
        public static double NextDouble(this IRNG<int> rng) => unchecked((uint)rng.Next()) * (1.0 / uint.MaxValue);
        public static double NextDouble(this IRNG<ulong> rng) => rng.Next() * (1.0 / ulong.MaxValue);
        public static double NextDouble(this IRNG<long> rng) => unchecked((ulong) rng.Next()) * (1.0 / ulong.MaxValue);

        public static uint Next(this IRNG<uint> rng, uint min, uint max) =>
            min >= max ? throw new ArgumentException(nameof(min)) : (uint)(min + rng.NextDouble() * (max - min));
        public static int Next(this IRNG<int> rng, int min, int max) =>
            min >= max ? throw new ArgumentException(nameof(min)) : (int)(min + rng.NextDouble() * (max - min));
        public static ulong Next(this IRNG<ulong> rng, ulong min, ulong max) =>
            min >= max ? throw new ArgumentException(nameof(min)) : (ulong)(min + rng.NextDouble() * (max - min));
        public static long Next(this IRNG<long> rng, long min, long max) =>
            min >= max ? throw new ArgumentException(nameof(min)) : (long)(min + rng.NextDouble() * (max - min));

        public static uint Next(this IRNG<uint> rng, uint max) =>
            0 < max ? (uint)(rng.NextDouble() * max) : throw new ArgumentException(nameof(max));
        public static int Next(this IRNG<int> rng, int max) =>
            0 < max ? (int)(rng.NextDouble() * max) : throw new ArgumentException(nameof(max));
        public static ulong Next(this IRNG<ulong> rng, ulong max) =>
            0 < max ? (ulong)(rng.NextDouble() * max) : throw new ArgumentException(nameof(max));
        public static long Next(this IRNG<long> rng, long max) =>
            0 < max ? - (long)(rng.NextDouble() * max) : throw new ArgumentException(nameof(max));

        public static void Fill(this IRNG<int> rng, byte[] buffer) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;
            for (int i = 0; i < len; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<uint> rng, byte[] buffer) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;
            for (int i = 0; i < len; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<long> rng, byte[] buffer) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;
            for (int i = 0; i < len; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<ulong> rng, byte[] buffer) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;
            for (int i = 0; i < len; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<int> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;
            for (int i = start; i < end; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<uint> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;
            for (int i = start; i < end; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<long> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;
            for (int i = start; i < end; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static void Fill(this IRNG<ulong> rng, byte[] buffer, int start, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var len = buffer.Length;

            if (start < 0) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + count > len) throw new ArgumentOutOfRangeException(nameof(count));

            int end = start + count;
            for (int i = start; i < end; ++i) buffer[i] = (byte)(rng.Next() | 0xFF);
        }

        public static byte[] NextBytes(this IRNG<int> rng, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return new byte[0];

            var rv = new byte[count];
            rng.Fill(rv);
            return rv;
        }

        public static byte[] NextBytes(this IRNG<uint> rng, int count) {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return new byte[0];

            var rv = new byte[count];
            rng.Fill(rv);
            return rv;
        }

        public static byte[] NextBytes(this IRNG<long> rng, int count) {
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

        public static T Choose<T>(this IList<T> source) {
            return source[_defaultSr.Next(0, source.Count)];
        }
    }
}
