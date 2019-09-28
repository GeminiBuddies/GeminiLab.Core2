using System;
using System.Collections.Generic;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using GeminiLab.Core2.Collections.Comparers;
using GeminiLab.Core2.Random;
using GeminiLab.Core2.Yielder;
using Xunit;

namespace XUnitTester.GeminiLab_Core2.Collections {
    public class PriorityQueueTest {
        internal static ulong Ceil2(ulong v) {
            unchecked {
                if ((v & (v - 1)) == 0) return v;

                while ((v & (v - 1)) != 0) v &= (v - 1);
                return v << 1;
            }
        }

        [Fact]
        public void PQueueTest() {
            // initializing...
            var list = DefaultRNG.I32.Take(777).ToList();
            var pq = new PriorityQueue<int>(list);

            // adding items...
            1048576.Times(() => {
                var x = DefaultRNG.Next();
                pq.Add(x);
                list.Add(x);
            });

            1025.Times(() => {
                var x = DefaultRNG.I32.Take(1023).ToList();
                pq.AddRange(x);
                list.AddRange(x);
            });

            // check it.
            list.Sort();
            Assert.Equal(list, pq.ToList());

            int len = 777 + 1048576 + 1025 * 1023;
            Assert.Equal(len, pq.Count);
            Assert.Equal((int)Ceil2((ulong)len), pq.Capacity);

            // empty pq.
            for (int i = 0; i < len; ++i) {
                Assert.Equal(list[i], pq.Peek());
                Assert.Equal(list[i], pq.Pop());
            }

            Assert.Equal(0, pq.Count);

            // it's empty now.
            Assert.Throws<InvalidOperationException>(() => { pq.Peek(); });
            Assert.Throws<InvalidOperationException>(() => { pq.Pop(); });
            Assert.Empty(pq.ToArray());
            Assert.Equal((int)Ceil2((ulong)len), pq.Capacity);


            31.Times(() => {
                var x = DefaultRNG.I32.Take(33).ToList();
                pq.AddRange(x);
                list.AddRange(x);
            });

            Assert.Equal((int)Ceil2((ulong)len), pq.Capacity);
            Assert.Throws<ArgumentOutOfRangeException>(() => { pq.Capacity = PriorityQueue<int>.MinimumItemsLength; });
            
            pq.Clear();
            Assert.Equal(0, pq.Count);
            Assert.Equal((int)Ceil2((ulong)len), pq.Capacity);

            pq.Reset();
            Assert.Equal(0, pq.Count);
            Assert.Equal(PriorityQueue<int>.MinimumItemsLength, pq.Capacity);
        }

        [Fact]
        public void Constructors() {
            var pq0 = new PriorityQueue<Index>();
            var pq1 = new PriorityQueue<string>(new[] { "1", "23" });
            var pq2 = new PriorityQueue<double>(Comparer<double>.Default.Reverse());

            pq2.Add(-123.4);
            pq2.Add(123.4);
            pq2.Add(0.0);

            Assert.Equal(123.4, pq2.Pop());
            Assert.Equal(0.0, pq2.Pop());
            Assert.Equal(-123.4, pq2.Pop());
        }
    }
}
