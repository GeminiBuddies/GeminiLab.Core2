using System;
using System.Security.Cryptography;
using GeminiLab.Core2.Yielder;

namespace GeminiLab.Core2.Random {
    internal static class InternalEntropy {
        private class InternalEntropyImpl : IRNG<ulong> {
            private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

            ulong IYielder<ulong>.Next() {
                lock (this) {
                    var temp =  new byte[8];
                    _rng.GetBytes(temp);
                    ulong rv;

                    unsafe {
                        fixed (byte* b = &temp[0]) rv = *(ulong*)b;
                    }

                    return rv;
                }
            }
        }

        public static IRNG<ulong> Instance { get; } = new InternalEntropyImpl();

        public static ulong Next() => Instance.Next();
    }
}
