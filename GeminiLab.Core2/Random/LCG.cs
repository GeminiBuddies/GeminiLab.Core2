using System;

namespace GeminiLab.Core2.Random {
    internal static class LCGUtil {
        internal static ulong StepModULong(ref ulong X, ulong a, ulong c) => X = unchecked(a * X + c);
        internal static uint StepModUInt(ref uint X, uint a, uint c) => X = unchecked(a * X + c);
    }
    
    public class LCG : IPRNG<uint> {
        protected internal uint a, c;
        private uint X;

        public LCG(uint a, uint c, uint seed) {
            this.a = a; this.c = c;
            Seed(seed);
        }

        public void Seed(uint seed) => X = seed;
        public uint Next() => LCGUtil.StepModUInt(ref X, a, c);
    }

    public class LCGA : LCG {
        private const uint A = 1664525u, C = 1013904223u;
        public LCGA(uint seed) : base(A, C, seed) { }
    }

    public class LCGB : LCG {
        private const uint A = 22695477u, C = 1u;
        public LCGB(uint seed) : base(A, C, seed) { }
    }

    public class LCGC : LCG {
        private const uint A = 134775813u, C = 1u;
        public LCGC(uint seed) : base(A, C, seed) { }
    }

    public class LCGD : LCG {
        private const uint A = 214013u, C = 2531011u;
        public LCGD(uint seed) : base(A, C, seed) { }
    }

    public class LCGx64 : IPRNG<ulong> {
        protected internal ulong a = 0, c = 0;
        private ulong X;

        public LCGx64(ulong a, ulong c, ulong seed) {
            this.a = a; this.c = c;
            Seed(seed);
        }

        public void Seed(ulong seed) => X = seed;
        public ulong Next() => LCGUtil.StepModULong(ref X, a, c);
    }

    public class LCGX64A : LCGx64 {
        private const ulong A = 6364136223846793005ul, C = 1442695040888963407ul;
        public LCGX64A(uint seed) : base(A, C, seed) { }
    }

    public class LCGX64B : LCGx64 {
        private const ulong A = 6364136223846793005ul, C = 1ul;
        public LCGX64B(uint seed) : base(A, C, seed) { }
    }
}
