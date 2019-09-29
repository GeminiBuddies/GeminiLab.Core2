using System.Diagnostics.CodeAnalysis;

namespace GeminiLab.Core2.Random.RNG {
    [ExcludeFromCodeCoverage]
    public class U32ToI32RNG<TU32RNG> : IRNG<int> where TU32RNG : IRNG<uint>, new() {
        protected readonly TU32RNG InternalRNG;

        public U32ToI32RNG(TU32RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public U32ToI32RNG() {
            InternalRNG = new TU32RNG();
        }

        public int Next() => unchecked((int)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U32ToI32PRNG<TU32PRNG> : U32ToI32RNG<TU32PRNG>, IPRNG<int> where TU32PRNG : IPRNG<uint>, new() {
        public U32ToI32PRNG(TU32PRNG internalRNG) : base(internalRNG) { }

        public U32ToI32PRNG() : base() { }

        public U32ToI32PRNG(int seed) : base() => Seed(seed);

        public void Seed(int seed) => InternalRNG.Seed(unchecked((uint)seed));
    }

    [ExcludeFromCodeCoverage]
    public class U32ToI32RNG : IRNG<int> {
        protected readonly IRNG<ulong> InternalRNG;

        public U32ToI32RNG(IRNG<ulong> internalRNG) {
            InternalRNG = internalRNG;
        }
        
        public int Next() => unchecked((int)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U32ToI32PRNG : U32ToI32RNG, IPRNG<int> {
        public U32ToI32PRNG(IPRNG<ulong> internalRNG) : base(internalRNG) { }

        public void Seed(int seed) => (InternalRNG as IPRNG<ulong>)!.Seed(unchecked((uint)seed));
    }

    [ExcludeFromCodeCoverage]
    public class I32ToU32RNG<TI32RNG> : IRNG<uint> where TI32RNG : IRNG<int>, new() {
        protected readonly TI32RNG InternalRNG;

        public I32ToU32RNG(TI32RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public I32ToU32RNG() {
            InternalRNG = new TI32RNG();
        }

        public uint Next() => unchecked((uint)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class I32ToU32PRNG<TI32PRNG> : I32ToU32RNG<TI32PRNG>, IPRNG<uint> where TI32PRNG : IPRNG<int>, new() {
        public I32ToU32PRNG(TI32PRNG internalRNG) : base(internalRNG) { }

        public I32ToU32PRNG() : base() { }

        public I32ToU32PRNG(uint seed) : base() => Seed(seed);

        public void Seed(uint seed) => InternalRNG.Seed(unchecked((int)seed));
    }

    [ExcludeFromCodeCoverage]
    public class I32ToU32RNG : IRNG<uint> {
        protected readonly IRNG<int> InternalRNG;

        public I32ToU32RNG(IRNG<int> internalRNG) {
            InternalRNG = internalRNG;
        }

        public uint Next() => unchecked((uint)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class I32ToU32PRNG : I32ToU32RNG, IPRNG<uint> {
        public I32ToU32PRNG(IPRNG<int> internalRNG) : base(internalRNG) { }

        public void Seed(uint seed) => (InternalRNG as IPRNG<int>)!.Seed(unchecked((int)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI64RNG<TU64RNG> : IRNG<long> where TU64RNG : IRNG<ulong>, new() {
        protected readonly TU64RNG InternalRNG;

        public U64ToI64RNG(TU64RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public U64ToI64RNG() {
            InternalRNG = new TU64RNG();
        }

        public long Next() => unchecked((long)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI64PRNG<TU64PRNG> : U64ToI64RNG<TU64PRNG>, IPRNG<long> where TU64PRNG : IPRNG<ulong>, new() {
        public U64ToI64PRNG(TU64PRNG internalRNG) : base(internalRNG) { }

        public U64ToI64PRNG() : base() { }

        public U64ToI64PRNG(long seed) : base() => Seed(seed);

        public void Seed(long seed) => InternalRNG.Seed(unchecked((ulong)seed));
    }

    [ExcludeFromCodeCoverage]
    public class U64ToI64RNG : IRNG<long> {
        protected readonly IRNG<ulong> InternalRNG;

        public U64ToI64RNG(IRNG<ulong> internalRNG) {
            InternalRNG = internalRNG;
        }

        public long Next() => unchecked((long)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI64PRNG : U64ToI64RNG, IPRNG<long> {
        public U64ToI64PRNG(IPRNG<ulong> internalRNG) : base(internalRNG) { }

        public void Seed(long seed) => (InternalRNG as IPRNG<ulong>)!.Seed(unchecked((ulong)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class I64ToU64RNG<TI64RNG> : IRNG<ulong> where TI64RNG : IRNG<long>, new() {
        protected readonly TI64RNG InternalRNG;

        public I64ToU64RNG(TI64RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public I64ToU64RNG() {
            InternalRNG = new TI64RNG();
        }

        public ulong Next() => unchecked((ulong)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class I64ToU64PRNG<TI64PRNG> : I64ToU64RNG<TI64PRNG>, IPRNG<ulong> where TI64PRNG : IPRNG<long>, new() {
        public I64ToU64PRNG(TI64PRNG internalRNG) : base(internalRNG) { }

        public I64ToU64PRNG() : base() { }

        public I64ToU64PRNG(ulong seed) : base() => Seed(seed);

        public void Seed(ulong seed) => InternalRNG.Seed(unchecked((long)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class I64ToU64RNG : IRNG<ulong> {
        protected readonly IRNG<long> InternalRNG;

        public I64ToU64RNG(IRNG<long> internalRNG) {
            InternalRNG = internalRNG;
        }

        public ulong Next() => unchecked((ulong)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class I64ToU64PRNG : I64ToU64RNG, IPRNG<ulong> {
        public I64ToU64PRNG(IPRNG<long> internalRNG) : base(internalRNG) { }

        public void Seed(ulong seed) => (InternalRNG as IPRNG<long>)!.Seed(unchecked((long)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class I32ToU64RNG<TI32RNG> : IRNG<ulong> where TI32RNG : IRNG<int>, new() {
        protected readonly TI32RNG InternalRNG;

        public I32ToU64RNG(TI32RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public I32ToU64RNG() {
            InternalRNG = new TI32RNG();
        }

        public ulong Next() {
            ulong rv;

            unsafe {
                *(int*)&rv = InternalRNG.Next();
                *(1 + (int*)&rv) = InternalRNG.Next();
            }

            return rv;
        }
    }
    
    [ExcludeFromCodeCoverage]
    public class I32ToU64PRNG<TI32PRNG> : I32ToU64RNG<TI32PRNG>, IPRNG<ulong> where TI32PRNG : IPRNG<int>, new() {
        public I32ToU64PRNG(TI32PRNG internalRNG) : base(internalRNG) { }

        public I32ToU64PRNG() : base() { }

        public I32ToU64PRNG(ulong seed) : base() => Seed(seed);

        public void Seed(ulong seed) => InternalRNG.Seed(unchecked((int)seed));
    }

    [ExcludeFromCodeCoverage]
    public class I32ToU64RNG : IRNG<ulong> {
        protected readonly IRNG<int> InternalRNG;

        public I32ToU64RNG(IRNG<int> internalRNG) {
            InternalRNG = internalRNG;
        }

        public ulong Next() {
            ulong rv;

            unsafe {
                *(int*)&rv = InternalRNG.Next();
                *(1 + (int*)&rv) = InternalRNG.Next();
            }

            return rv;
        }
    }
    
    [ExcludeFromCodeCoverage]
    public class I32ToU64PRNG : I32ToU64RNG, IPRNG<ulong> {
        public I32ToU64PRNG(IPRNG<int> internalRNG) : base(internalRNG) { }

        public void Seed(ulong seed) => (InternalRNG as IPRNG<int>)!.Seed(unchecked((int)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI32RNG<TU64RNG> : IRNG<int> where TU64RNG : IRNG<ulong>, new() {
        protected readonly TU64RNG InternalRNG;

        public U64ToI32RNG(TU64RNG internalRNG) {
            InternalRNG = internalRNG;
        }

        public U64ToI32RNG() {
            InternalRNG = new TU64RNG();
        }

        public int Next() => unchecked((int)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI32PRNG<TU64PRNG> : U64ToI32RNG<TU64PRNG>, IPRNG<int> where TU64PRNG : IPRNG<ulong>, new() {
        public U64ToI32PRNG(TU64PRNG internalRNG) : base(internalRNG) { }

        public U64ToI32PRNG() : base() { }

        public U64ToI32PRNG(int seed) : base() => Seed(seed);

        public void Seed(int seed) => InternalRNG.Seed(unchecked((ulong)seed));
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI32RNG : IRNG<int> {
        protected readonly IRNG<ulong> InternalRNG;

        public U64ToI32RNG(IRNG<ulong> internalRNG) {
            InternalRNG = internalRNG;
        }

        public int Next() => unchecked((int)InternalRNG.Next());
    }
    
    [ExcludeFromCodeCoverage]
    public class U64ToI32PRNG : U64ToI32RNG, IPRNG<int> {
        public U64ToI32PRNG(IPRNG<ulong> internalRNG) : base(internalRNG) { }

        public void Seed(int seed) => (InternalRNG as IPRNG<ulong>)!.Seed(unchecked((ulong)seed));
    }
}
