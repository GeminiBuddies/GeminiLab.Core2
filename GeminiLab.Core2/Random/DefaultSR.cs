using System;

using SR = System.Random;

namespace GeminiLab.Core2.Random {
    internal static class DefaultSr {
        internal static readonly SR Sr = new SR(
            Environment.CurrentManagedThreadId ^ (Environment.TickCount << 16) ^ DateTime.UtcNow.ToString("fffssmmHHyyyyMMdd").GetHashCode()
        );
    }
}
