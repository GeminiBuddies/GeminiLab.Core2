using System;

namespace GeminiLab.Core2.Random {
    public static class DefaultRNG {
        private static readonly IRNG<int> Drng = new Mt19937S(
            Environment.CurrentManagedThreadId ^ (Environment.TickCount << 16) ^ DateTime.UtcNow.ToString("fffssmmHHyyyyMMdd").GetHashCode()
        );

        public static int GetNext(int minValue, int maxValue) {
            lock (Drng) {
                return Drng.Next(minValue, maxValue);
            }
        }

        public static int GetNext() {
            lock (Drng) {
                return Drng.Next();
            }
        }

        public static ulong GetNextULong() {
            lock (Drng) {
                var low = (ulong)GetNext();
                var high = (ulong)GetNext();

                return (high << 32) | low;
            }
        }

        public static IRNG<int> Instance { get; } = new DefaultIntRNG();

        private static Coin Coin { get; } = new Coin();
    }

    internal class DefaultIntRNG : IRNG<int> {
        public int Next() {
            return DefaultRNG.GetNext();
        }
    }

    internal class Coin : IRNG<bool> {
        public bool Next() {
            return DefaultRNG.GetNext() % 2 == 0;
        }
    }
}
