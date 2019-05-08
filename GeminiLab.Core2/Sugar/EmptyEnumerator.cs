using System;
using System.Collections;
using System.Collections.Generic;

namespace GeminiLab.Core2.Sugar {
    public class EmptyEnumerator<T> : IEnumerator<T> {
        public bool MoveNext() => false;

        public void Reset() { }

        public T Current => throw new InvalidOperationException();
        object IEnumerator.Current => Current;

        public void Dispose() { }

        private EmptyEnumerator() { }

        private static EmptyEnumerator<T> _shardOne = null;
        public static EmptyEnumerator<T> Instance => _shardOne ?? (_shardOne = new EmptyEnumerator<T>());
    }
}
