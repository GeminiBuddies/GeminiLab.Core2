using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections.Comparers {
    public class LambdaComparer<T> : IComparer<T> {
        private readonly Func<T, T, int> _lambdaInt;

        public LambdaComparer(Func<T, T, int> lambda) {
            _lambdaInt = lambda ?? throw new ArgumentNullException();
        }

        public LambdaComparer(Func<T, T, bool> lessThan) {
            if (lessThan == null) throw new ArgumentNullException(nameof(lessThan));

            _lambdaInt = (x, y) => lessThan(x, y) ? -1 : (lessThan(y, x) ? 1 : 0);
        }

        public int Compare(T x, T y) => _lambdaInt(x, y);
    }
}
