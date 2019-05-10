using System;

namespace GeminiLab.Core2.Random {
    public static class DefaultRNG {
        private static readonly IRNG<ulong> InnerOne = new Mt19937X64(
            (1ul << (Environment.CurrentManagedThreadId & 0x3F)) ^ 
            unchecked((ulong)Environment.TickCount << 32) ^ 
            unchecked((ulong)DateTime.UtcNow.ToString("fffssmmHHyyyyMMdd").GetHashCode()) ^ 
            InternalEntropy.Next()
        );
        
        public static int Next() {
            lock (InnerOne) {
                return unchecked((int)(InnerOne.Next() & 0xFFFFFFFF));
            }
        }

        public static ulong NextULong() {
            lock (InnerOne) {
                return InnerOne.Next();
            }
        }

        public static double NextDouble() {
            lock (InnerOne) {
                return InnerOne.NextDouble();
            }
        }

        public static IRNG<int> Instance { get; } = new DefaultIntRNG();

        public static IRNG<bool> Coin { get; } = new Coin();
    }

    internal class DefaultIntRNG : IRNG<int> {
        public int Next() {
            return DefaultRNG.Next();
        }
    }

    internal class Coin : IRNG<bool> {
        public bool Next() {
            return DefaultRNG.Next() % 2 == 0;
        }
    }
}
