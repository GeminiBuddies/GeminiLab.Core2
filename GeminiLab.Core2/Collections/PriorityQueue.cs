using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Collections {
    // just a heap
    // not thread-safe now
    // implementing ICollection<T> is too complex
    public class PriorityQueue<T> {
        private const long DefaultItemsLength = 16;

        private long _size, _cap;
        private T[] _items;

        private readonly IComparer<T> _comp;

        public long Count => _size;
        public long Capacity => _cap;

        private PriorityQueue(long size, long cap, T[] items, IComparer<T> comp) {
            _size = size;
            _cap = cap;
            _items = new T[items.Length];
            Array.Copy(items, _items, items.Length);
            _comp = comp;
        }

        public PriorityQueue() {
            _size = 0;
            _cap = DefaultItemsLength;

            _items = new T[DefaultItemsLength];
            _comp = Comparer<T>.Default;
        }

        public PriorityQueue(IComparer<T> comparer) {
            _size = 0;
            _cap = DefaultItemsLength;

            _items = new T[DefaultItemsLength];
            _comp = comparer ?? Comparer<T>.Default;
        }

        public PriorityQueue(IEnumerable<T> items) : this() {
            AddRange(items);
        }

        public PriorityQueue(IEnumerable<T> items, IComparer<T> comparer) : this(comparer) {
            AddRange(items);
        }

        private void doubleRange() {
            var newCap = _cap * 2;
            var newItems = new T[newCap];

            Array.Copy(_items, newItems, _cap);
            _items = newItems;
            _cap = newCap;
        }

        public void Add(T item) {
            if (_size == (_cap - 1)) doubleRange();

            ++_size;
            long ptr = _size;
            while (ptr > 1 && _comp.Compare(item, _items[ptr >> 1]) < 0) {
                _items[ptr] = _items[ptr >> 1];
                ptr >>= 1;
            }

            _items[ptr] = item;
        }

        public void AddRange(IEnumerable<T> items) {
            foreach (var i in items) Add(i);
        }

        public void RemoveHead() {
            if (_size <= 0) throw new InvalidOperationException();

            if (_size != 1) {
                --_size;

                long ptr = 1;
                T last = _items[_size + 1];
                while (true) {
                    if (ptr << 1 > _size) break;

                    if (_comp.Compare(_items[ptr << 1], last) < 0) {
                        if ((ptr << 1) + 1 <= _size && _comp.Compare(_items[(ptr << 1) + 1], _items[ptr << 1]) < 0) {
                            _items[ptr] = _items[(ptr << 1) + 1]; ptr = (ptr << 1) + 1;
                        } else {
                            _items[ptr] = _items[ptr << 1]; ptr <<= 1;
                        }
                    } else if ((ptr << 1) + 1 <= _size && _comp.Compare(_items[(ptr << 1) + 1], last) < 0) {
                        _items[ptr] = _items[(ptr << 1) + 1]; ptr = (ptr << 1) + 1;
                    } else {
                        break;
                    }
                }

                _items[ptr] = last;
            } else {
                _size = 0;
            }
        }

        public T Peek() {
            return _size > 0 ? _items[1] : throw new InvalidOperationException();
        }

        public T Pop() {
            T rv = Peek();
            RemoveHead();
            return rv;
        }

        // caution! very slow.
        public List<T> AsList() {
            var newQueue = new PriorityQueue<T>(_size, _cap, _items, _comp);
            var rv = new List<T>((int)_size);

            for (int i = 0; i < _size; ++i) rv.Add(newQueue.Pop());
            return rv;
        }
    }
}
