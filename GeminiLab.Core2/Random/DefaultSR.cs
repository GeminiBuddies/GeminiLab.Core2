using System;

using SR = System.Random;

namespace GeminiLab.Core2.Random {
    internal static class DefaultSr {
        private static readonly SR Sr = new SR(
            Environment.CurrentManagedThreadId ^ (Environment.TickCount << 16) ^ DateTime.UtcNow.ToString("fffssmmHHyyyyMMdd").GetHashCode()
        );

        internal static int GetNext(int minValue, int maxValue) {
            lock (Sr) {
                return Sr.Next(minValue, maxValue);
            }
        }

        internal static int GetNext() {
            lock (Sr) {
                return Sr.Next(int.MinValue, int.MaxValue);
            }
        }

        internal static ulong GetNextULong() {
            lock (Sr) {
                var low = (ulong)GetNext();
                var high = (ulong)GetNext();

                return (high << 32) | low;
            }
        }
    }
}
