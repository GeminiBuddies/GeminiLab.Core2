using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2 {
    public interface IInfiniteEnumerable<out T> {
        IInfiniteEnumerator<T> GetEnumerator();
    }

    public interface IInfiniteEnumerator<out T> {
        void Reset();
        T GetNext();
    }
}
