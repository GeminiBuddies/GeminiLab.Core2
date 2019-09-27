using System;
using System.Collections.Generic;
using System.Linq;

namespace GeminiLab.Core2.Random {
    public class Chooser<TValue> : IRNG<TValue> {
        private readonly IList<TValue> _values;
        private readonly int _count;
        private readonly IRNG<int> _rng;
        
        private Chooser(IList<TValue> values, IRNG<int> rng) {
            _values = values;
            _count = values.Count;
            _rng = rng;
        }

        public Chooser(IEnumerable<TValue> values) : this(values is IList<TValue> list ? list : values.ToArray(), DefaultRNG.I32) { }

        public Chooser(IEnumerable<TValue> values, IRNG<int> rng) : this(values is IList<TValue> list ? list : values.ToArray(), rng) { }

        public TValue Next() {
            lock (this) {
                return _values[_rng.Next(_count)];
            }
        }
    }

    public static class Chooser {
        public static Chooser<T> Make<T>(IEnumerable<T> value) {
            return new Chooser<T>(value ?? throw new ArgumentNullException(nameof(value)));
        }

        public static Chooser<T> Make<T>(IEnumerable<T> value, IRNG<int> rng) {
            return new Chooser<T>(value ?? throw new ArgumentNullException(nameof(value)), rng ?? throw new ArgumentNullException(nameof(rng)));
        }

        public static T Choose<T>(this IList<T> source) {
            return source[DefaultRNG.I32.Next(0, source.Count)];
        }

        public static Chooser<T> MakeChooser<T>(this IEnumerable<T> source) => Chooser.Make(source);

        public static Chooser<T> MakeChooser<T>(this IEnumerable<T> source, IRNG<int> rng) => Chooser.Make(source, rng);
    }
}
