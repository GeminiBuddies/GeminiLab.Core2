namespace GeminiLab.Core2.Random {
    public class Mt19937X64 : IPRNG<ulong> {
        private const int W = 64;
        private const ulong N = 312;
        private const ulong M = 156;
        private const int R = 31;
        private const ulong A = 0xB5026F5AA96619E9;
        private const int U = 29;
        private const ulong D = 0x5555555555555555;
        private const int S = 17;
        private const ulong B = 0x71D67FFFEDA60000;
        private const int T = 37;
        private const ulong C = 0xFFF7EEE000000000;
        private const int L = 43;
        private const ulong F = 6364136223846793005;

        public const ulong LowerMask = (1ul << R) - 1;
        public const ulong UpperMask = ~LowerMask;

        private readonly ulong[] _mt = new ulong[N];
        private ulong _index;

        public Mt19937X64(ulong seed) => Seed(seed);
        public Mt19937X64() : this(DefaultRNG.GetNextULong()) { }

        public void Seed(ulong seed) {
            lock (this) {
                _index = N;
                _mt[0] = seed;

                for (ulong i = 1; i < N; ++i) {
                    _mt[i] = (F * (_mt[i - 1] ^ (_mt[i - 1] >> (W - 2))) + i);
                }
            }
        }

        public ulong Next() {
            lock (this) {
                if (_index >= N) twist();

                ulong y = _mt[_index];
                y = y ^ ((y >> U) & D);
                y = y ^ ((y << S) & B);
                y = y ^ ((y << T) & C);
                y = y ^ (y >> L);

                ++_index;

                return y;
            }
        }

        private void twist() {
            for (ulong i = 0; i < N; ++i) {
                ulong x = (_mt[i] & UpperMask) | (_mt[(i + 1) % N] & LowerMask);
                ulong xA = x >> 1;

                if ((x & 1) == 1) xA = xA ^ A;

                _mt[i] = _mt[(i + M) % N] ^ xA;
            }

            _index = 0;
        }
    }

    public class Mt19937 : IPRNG<uint> {
        private const int W = 32;
        private const uint N = 624;
        private const uint M = 397;
        private const int R = 31;
        private const uint A = 0x9908B0DF;
        private const int U = 11;
        private const uint D = 0xffffffff;
        private const int S = 7;
        private const uint B = 0x9d2c5680;
        private const int T = 15;
        private const uint C = 0xefc60000;
        private const int L = 18;
        private const uint F = 1812433253;

        public const uint LowerMask = (1u << R) - 1;
        public const uint UpperMask = ~LowerMask;

        private readonly uint[] _mt = new uint[N];
        private uint _index;

        public Mt19937(uint seed) => Seed(seed);
        public Mt19937() : this((uint)DefaultRNG.GetNext()) { }

        public void Seed(uint seed) {
            lock (this) {
                _index = N;
                _mt[0] = seed;

                for (uint i = 1; i < N; ++i) {
                    _mt[i] = (F * (_mt[i - 1] ^ (_mt[i - 1] >> (W - 2))) + i);
                }
            }
        }

        public uint Next() {
            lock (this) {
                if (_index >= N) twist();

                uint y = _mt[_index];
                y = y ^ ((y >> U) & D);
                y = y ^ ((y << S) & B);
                y = y ^ ((y << T) & C);
                y = y ^ (y >> L);

                ++_index;

                return y;
            }
        }

        private void twist() {
            for (uint i = 0; i < N; ++i) {
                uint x = (_mt[i] & UpperMask) | (_mt[(i + 1) % N] & LowerMask);
                uint xA = x >> 1;

                if ((x & 1) == 1) xA = xA ^ A;

                _mt[i] = _mt[(i + M) % N] ^ xA;
            }

            _index = 0;
        }
    }

    public class Mt19937S : IPRNG<int> {
        private readonly Mt19937 _gen;

        public Mt19937S(int seed) => _gen = new Mt19937(unchecked((uint)seed));
        public Mt19937S() : this(DefaultRNG.GetNext()) { }

        public void Seed(int seed) => _gen.Seed(unchecked((uint)seed));
        public int Next() => unchecked((int)_gen.Next());
    }

    public class Mt19937X64S : IPRNG<long> {
        private readonly Mt19937X64 _gen;

        public Mt19937X64S(long seed) => _gen = new Mt19937X64(unchecked((ulong)seed));
        public Mt19937X64S() : this(DefaultRNG.GetNext()) { }

        public void Seed(long seed) => _gen.Seed(unchecked((ulong)seed));
        public long Next() => unchecked((long)_gen.Next());
    }
}
