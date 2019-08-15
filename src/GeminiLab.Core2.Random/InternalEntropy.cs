/*
 * This class provides CSPRNG(cryptographically secure pseudorandom number generator)
 * This class is temporarily removed because
 * - it's costly
 * - it references too may libs
 * - using CSPRN as internal entropy to initialize default rng is no more secure than initializing it with some runtime information.
 */

/*
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

        public static IRNG<ulong> I32 { get; } = new InternalEntropyImpl();

        public static ulong Next() => I32.Next();
    }
}

*/
