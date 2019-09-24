namespace GeminiLab.Core2.Random.RNG {
    public class PCG : IPRNG<int, ulong> {
        private readonly ulong _inc;
        private ulong _state;

        public PCG() : this(DefaultRNG.NextU64(), DefaultRNG.NextU64()) {}

        public PCG(ulong seq) {
            _inc = (seq << 1) | 1;
        }

        public PCG(ulong seq, ulong seed) : this(seq) {
            Seed(seed);
        }

        private static uint ror(uint x, uint r) {
            return (x >> (int)r) | (x << (int)(-r & 31));
        }

        public int Next() {
            var x = _state;

            unchecked {
                _state = _state * 6364136223846793005ul + _inc;
                return (int)ror((uint)(((x >> 18) ^ x) >> 27), (uint)(x >> 59));
            }
        }

        public void Seed(ulong seed) {
            _state = 0ul;
            Next();
            _state += seed;
            Next();
        }
    }
}
