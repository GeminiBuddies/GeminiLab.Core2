using System;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Random {
    public class Chooser<TValue, TRNG> : IPRNG<TValue, int> where TRNG : IPRNG<int>, new() {
        private int _seed;
        private readonly IList<TValue> _values;
        private readonly int _count;
        private ChooserEnumerator _defaultEnumerator;
        private volatile int _version;

        public Chooser(IList<TValue> values) : this(values, DefaultRNG.Next()) { }

        public Chooser(IList<TValue> values, int seed) {
            _seed = seed;
            _values = values;
            _count = values.Count;
            _version = 0;

            _defaultEnumerator = getEnumeratorPrivate();
        }

        public Chooser(IEnumerable<TValue> values) : this(values is IList<TValue> ? (IList<TValue>)values : values.ToArray()) { }

        public Chooser(IEnumerable<TValue> values, int seed) : this(values is IList<TValue> ? (IList<TValue>) values : values.ToArray(), seed) { }

        private ChooserEnumerator getEnumeratorPrivate() {
            lock (this) {
                return new ChooserEnumerator(this, _version);
            }
        }


        public TValue Next() => _defaultEnumerator.GetNext();
        public void Reset() => _defaultEnumerator.Reset();

        public void Seed(int seed) {
            lock (this) {
                _seed = seed;
                _defaultEnumerator = new ChooserEnumerator(this, ++_version);
            }
        }

        private class ChooserEnumerator {
            private readonly Chooser<TValue, TRNG> _mother;
            private readonly TRNG _rng;
            private readonly int _version;

            internal ChooserEnumerator(Chooser<TValue, TRNG> mother, int version) {
                _mother = mother;
                _version = version;

                _rng = new TRNG();
                _rng.Seed(_mother._seed);
            }

            public TValue GetNext() {
                lock (_mother) {
                    if (_version != _mother._version) throw new InvalidOperationException();

                    return _mother._values[_rng.Next(_mother._count)];
                }
            }

            public void Reset() {
                lock (_mother) {
                    _rng.Seed(_mother._seed);
                }
            }
        }
    }

    public class Chooser<TValue> : Chooser<TValue, Mt19937S> {
        public Chooser(IList<TValue> values) : base(values) { }
        public Chooser(IList<TValue> values, int seed) : base(values, seed) { }
        public Chooser(IEnumerable<TValue> values) : base(values) { }
        public Chooser(IEnumerable<TValue> values, int seed) : base(values, seed) { }
    }

    public static class Chooser {
        public static Chooser<T> Make<T>(IEnumerable<T> value) {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return value is IList<T> ilist ? new Chooser<T>(ilist) : new Chooser<T>(value.ToArray());
        }

        public static T Choose<T>(this IList<T> source) {
            return source[DefaultRNG.Instance.Next(0, source.Count)];
        }

        public static Chooser<T> MakeChooser<T>(this IEnumerable<T> source) => Make(source);
    }
}
