namespace GeminiLab.Core2.Random.RNG {
    public sealed class LCG : IPRNG<uint> {
        public const uint A = 1664525u, C = 1013904223u;

        /* other candidates
        public const uint A = 22695477u, C = 1u;
        public const uint A = 134775813u, C = 1u;
        public const uint A = 214013u, C = 2531011u;
        */

        private readonly uint _a, _c;
        private uint _x;

        public LCG(uint a, uint c, uint seed) {
            _a = a; _c = c;
            Seed(seed);
        }

        public LCG(uint seed) : this(A, C, seed) { }
        public LCG() : this(unchecked((uint)DefaultRNG.Next())) { }

        public void Seed(uint seed) {
            lock (this) _x = seed;
        }

        public uint Next() {
            lock (this) return _x = unchecked(_a * _x + _c);
        }
    }

    public sealed class LCG64 : IPRNG<ulong> {
        public const ulong A = 6364136223846793005ul, C = 1442695040888963407ul;

        /* other candidates
        public const ulong A = 6364136223846793005ul, C = 1ul;
        */

        private readonly ulong _a, _c;
        private ulong _x;

        public LCG64(ulong a, ulong c, ulong seed) {
            _a = a; _c = c;
            Seed(seed);
        }

        public LCG64(ulong seed) : this(A, C, seed) { }
        public LCG64() : this(DefaultRNG.NextU64()) { }

        public void Seed(ulong seed) {
            lock (this) _x = seed;
        }

        public ulong Next() {
            lock (this) return _x = unchecked(_a * _x + _c);
        }
    }
}
