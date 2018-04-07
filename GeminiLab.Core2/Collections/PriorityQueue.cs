using System;
using System.Collections.Generic;
using System.Linq;

using GeminiLab.Core2.Collections.HeapBase;

namespace GeminiLab.Core2.Collections {
    // just a heap
    // not thread-safe
    // won't implement interfaces like IEnumerable<T>, ICollection<T>, etc.
    public class PriorityQueue<T> {
        private const long DefaultItemsLength = 16;

        private long _size, _cap;
        private T[] _items;

        private readonly IComparer<T> _comp;

        public long Count => _size;
        public long Capacity => _cap;
        
        public PriorityQueue() {
            _comp = Comparer<T>.Default;
            initWithoutArray();
        }

        public PriorityQueue(IComparer<T> comparer) {
            _comp = comparer ?? Comparer<T>.Default;
            initWithoutArray();
        }

        public PriorityQueue(IEnumerable<T> items) {
            _comp = Comparer<T>.Default;

            var arr = items as T[] ?? items.ToArray();
            initWithArray(arr);
        }

        public PriorityQueue(IEnumerable<T> items, IComparer<T> comparer) {
            _comp = comparer ?? Comparer<T>.Default;

            var arr = items as T[] ?? items.ToArray();
            initWithArray(arr);
        }

        private void initWithoutArray() {
            _size = 0;
            _cap = DefaultItemsLength;

            _items = new T[DefaultItemsLength];
        }

        private void initWithArray(T[] array) {
            _size = array.Length;
            _cap = _size < DefaultItemsLength ? DefaultItemsLength : ceil2(_size);

            _items = new T[_cap];
            Array.Copy(array, _items, _size);
            _items.MakeHeap(_size, _comp);
        }

        private void doubleRange() {
            var newCap = _cap * 2;
            var newItems = new T[newCap];

            Array.Copy(_items, newItems, _cap);
            _items = newItems;
            _cap = newCap;
        }

        public void Add(T item) {
            if (_size == _cap) doubleRange();

            _items[_size++] = item;
            _items.PushHeap(_size, _comp);
        }

        public void AddRange(IEnumerable<T> items) {
            foreach (var i in items) Add(i);
        }

        public void RemoveHead() {
            Pop();
        }

        public T Peek() {
            return _size > 0 ? _items[0] : throw new InvalidOperationException();
        }

        public T Pop() {
            if (_size == 0) throw new InvalidOperationException();

            _items.PopHeap(_size, _comp);
            var rv = _items[--_size];
            _items[_size] = default;
            return rv;
        }

        // caution! very slow.
        public List<T> AsList() {
            if (_size == 0) return new List<T>();

            var rv = new T[_size];
            Array.Copy(_items, rv, _size);
            rv.SortHeap(_size, _comp);
            return new List<T>(rv);
        }

        private static long ceil2(long v) {
            if (v <= 0) return 1;
            if ((v & (v - 1)) == 0) return v;

            while ((v & (v - 1)) != 0) v &= (v - 1);
            return v << 1;
        }
    }
}
