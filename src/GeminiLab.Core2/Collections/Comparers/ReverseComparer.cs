using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections.Comparers {
    public class ReverseComparer<T> : IComparer<T> {
        readonly IComparer<T> _internalComp;

        public ReverseComparer(IComparer<T> internalComp) {
            _internalComp = internalComp ?? throw new ArgumentNullException(nameof(internalComp));
        }
        
        int IComparer<T>.Compare(T x, T y) {
            int v = _internalComp.Compare(x, y);
            return (v == 0) ? 0 : (v > 0 ? -1 : 1);
        }
    }
}
