using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using GeminiLab.Core2.Collections.HeapBase;
using GeminiLab.Core2.Consts;

namespace GeminiLab.Core2.Collections {
    // just a heap
    // not thread-safe
    // won't implement interfaces like IEnumerable<T>, ICollection<T>, etc.
    public class PriorityQueue<T> {
        private const int MinimumItemsLength = 8;

        private int _size, _cap;
        private T[] _items;

        private readonly IComparer<T> _comp;

        private static int getLegalCapacity(int expected) {
            if (expected < MinimumItemsLength) return MinimumItemsLength;

            ulong p = GMath.Ceil2((ulong)expected);

            if (p > SystemArray.MaxArrayLength) return SystemArray.MaxArrayLength;
            return (int)p;
        }

        public int Count => _size;

        public int Capacity {
            get => _cap;
            set {
                if (value < _size) throw new ArgumentOutOfRangeException(nameof(value));

                var newCap = getLegalCapacity(value);
                if (newCap == _cap) return;

                
            }
        }

        public PriorityQueue(): this(Comparer<T>.Default) { }

        public PriorityQueue(IComparer<T> comparer): this(null, comparer) { }

        public PriorityQueue(IEnumerable<T>? items): this(items, Comparer<T>.Default) { }

        public PriorityQueue(IEnumerable<T>? items, IComparer<T> comparer) {
            _comp = comparer;

            if (items == null) {
                _size = 0;
                _cap = MinimumItemsLength;

                _items = new T[MinimumItemsLength];
            } else {
                var arr = items as T[] ?? items.ToArray();

                _size = arr.Length;
                _cap = getLegalCapacity(_size);

                _items = new T[_cap];
                Array.Copy(arr, _items, _size);
                _items.MakeHeap(_size, _comp);
            }
        }

        private void doubleRange() => Capacity *= 2;

        public void Clear() {
            _size = 0;
            Array.Clear(_items, 0, _cap);
        }

        public void Reset() {
            _size = 0;
            _cap = MinimumItemsLength;
            _items = new T[_cap];
        }

        public void Add(T item) {
            if (_size == _cap) doubleRange();

            _items[_size++] = item;
            _items.PushHeap(_size, _comp);
        }

        public void AddRange(IEnumerable<T> items) {
            foreach (var i in items) Add(i);
        }
        
        public T Peek() {
            return _size > 0 ? _items[0] : throw new InvalidOperationException();
        }

        public T Pop() {
            if (_size <= 0) throw new InvalidOperationException();

            _items.PopHeap(_size, _comp);
            var rv = _items[--_size];
            _items[_size] = default!;
            return rv;
        }

        // caution! very slow.
        public T[] ToArray() {
            if (_size == 0) return new T[0];

            var rv = new T[_size];
            Array.Copy(_items, rv, _size);
            rv.SortHeap(_size, _comp);
            return rv;
        }

        public List<T> ToList() => new List<T>(ToArray());
    }
}