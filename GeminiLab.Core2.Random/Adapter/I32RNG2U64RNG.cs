using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Random.Adapter {
    internal class I32RNG2U64RNG : IRNG<ulong> {
        private readonly IRNG<int> _rng;
        public I32RNG2U64RNG(IRNG<int> rng) {
            _rng = rng;
        }

        public ulong Next() {
            ulong rv;

            unsafe {
                *(int*)&rv = _rng.Next();
                *(1 + (int*)&rv) = _rng.Next();
            }

            return rv;
        }
    }
}
