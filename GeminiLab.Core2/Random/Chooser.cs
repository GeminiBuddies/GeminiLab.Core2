using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Random {
    public class Chooser<TValue, TRNG> : IPRNG<TValue, int>, IInfiniteEnumerable<TValue> where TRNG : IPRNG<int>, new() {
        private int _seed;
        private readonly IList<TValue> _values;
        private readonly int _count;
        private readonly ChooserEnumerator _defaultEnumerator;

        public Chooser(IList<TValue> values) : this(values, DefaultSr.Sr.Next(int.MinValue, int.MaxValue)) { }
        public Chooser(IList<TValue> values, int seed) {
            _seed = seed;
            _values = values;
            _count = values.Count;

            _defaultEnumerator = new ChooserEnumerator(this);
        }

        public IInfiniteEnumerator<TValue> GetEnumerator() => new ChooserEnumerator(this);

        public TValue Next() => _defaultEnumerator.GetNext();
        public void Reset() => _defaultEnumerator.Reset();

        public void Seed(int seed) {
            _seed = seed;
            _defaultEnumerator.Reset();
        }

        private class ChooserEnumerator : IInfiniteEnumerator<TValue> {
            private readonly Chooser<TValue, TRNG> _mother;
            private readonly TRNG _rng;

            internal ChooserEnumerator(Chooser<TValue, TRNG> mother) {
                _mother = mother;

                _rng = new TRNG();
                _rng.Seed(_mother._seed);
            }

            public TValue GetNext() => _mother._values[_rng.Next(_mother._count)];
            public void Reset() => _rng.Seed(_mother._seed);
        }
    }

    public class Chooser<TValue> : Chooser<TValue, Mt19937S> {
        public Chooser(IList<TValue> values) : base(values) { }
        public Chooser(IList<TValue> values, int seed) : base(values, seed) { }
    }

    public static class Chooser {
        public static Chooser<T> Make<T>(IEnumerable<T> value) {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            return value is IList<T> ilist ? new Chooser<T>(ilist) : new Chooser<T>(value.ToArray());
        }

        public static T Choose<T>(this IList<T> source) {
            return source[DefaultSr.Sr.Next(0, source.Count)];
        }
    }
}
