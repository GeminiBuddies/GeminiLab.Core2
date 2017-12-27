using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Random {
    // It's an infinity sequence, be careful when using it as IEnumerable
    // Do not try getting and keeping more than one enumerators of any instance of this class.
    // Do not reset enumerators of any instance of this class. It does not works.
    // Use an instance of this class as IEnumerable<TValue> only when you do not use this instance directly.
    public class Chooser<TValue, TRNG> : IPRNG<TValue, int>, IEnumerable<TValue> where TRNG : IPRNG<int>, new() {
        private readonly TRNG _rng;
        private readonly IList<TValue> _values;
        private readonly int _count;

        public Chooser(IList<TValue> values) {
            _rng = new TRNG();
            _values = values;
            _count = values.Count;
        }

        public Chooser(IList<TValue> values, int seed) : this(values) {
            Seed(seed);
        }

        public IEnumerator<TValue> GetEnumerator() => new ChooserEnumerator { Mother = this };
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public TValue Next() => _values[_rng.Next(_count)];
        public void Seed(int seed) => _rng.Seed(seed);

        private class ChooserEnumerator : IEnumerator<TValue> {
            private TValue _v;
            public Chooser<TValue, TRNG> Mother;

            public TValue Current => _v;
            object IEnumerator.Current => Current;

            public bool MoveNext() {
                _v = Mother.Next();
                return true;
            }

            public void Reset() { }
            public void Dispose() { }
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
    }
}
