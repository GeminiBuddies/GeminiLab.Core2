using System.Collections.Generic;

namespace GeminiLab.Core2.Random.Sugar {
    public static class ChooserExtensions {
        public static T Choose<T>(this IList<T> source) {
            return source[DefaultRNG.I32.Next(0, source.Count)];
        }

        public static Chooser<T> MakeChooser<T>(this IEnumerable<T> source) => Chooser.Make(source);

        public static Chooser<T> MakeChooser<T>(this IEnumerable<T> source, IRNG<int> rng) => Chooser.Make(source, rng);
    }
}
