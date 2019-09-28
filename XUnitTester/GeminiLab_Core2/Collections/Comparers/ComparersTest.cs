using System;
using System.Collections.Generic;
using System.Linq;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using GeminiLab.Core2.Collections.Comparers;
using GeminiLab.Core2.Random;
using Xunit;

using C = GeminiLab.Core2.Collections.Comparers.Comparers;

namespace XUnitTester.GeminiLab_Core2.Collections.Comparers {
    public class ComparersTest {
        private static IEnumerable<(int, int)> RandomIntPairs(int count) => count.Times(() => (DefaultRNG.Next(), DefaultRNG.Next()));
        private static IEnumerable<(int, int)> RandomSameIntPairs(int count) => count.Times(() => {
            var x = DefaultRNG.Next();
            return (x, x);
        });

        [Fact]
        public void LambdaComparerTest() {
            Func<int, int, bool> lessThan = (x, y) => x < y;
            var comp1 = lessThan.AsComparer();

            Assert.True(RandomIntPairs(32768).Select(tuple => comp1.Compare(tuple.Item1, tuple.Item2) == Comparer<int>.Default.Compare(tuple.Item1, tuple.Item2)).All());
            Assert.True(RandomSameIntPairs(32768).Select(tuple => comp1.Compare(tuple.Item1, tuple.Item2) == Comparer<int>.Default.Compare(tuple.Item1, tuple.Item2)).All());

            Func<int, int, int> rev = (x, y) => x < y ? 1 : x > y ? -1 : 0;
            var comp2 = rev.AsComparer();

            Assert.True(RandomIntPairs(32768).Select(tuple => comp2.Compare(tuple.Item1, tuple.Item2) + Comparer<int>.Default.Compare(tuple.Item1, tuple.Item2) == 0).All());
            Assert.True(RandomSameIntPairs(32768).Select(tuple => comp2.Compare(tuple.Item1, tuple.Item2) + Comparer<int>.Default.Compare(tuple.Item1, tuple.Item2) == 0).All());
        }

        [Fact]
        public void ReverseComparerTest() {
            var def = Comparer<int>.Default;
            var rev = def.Reverse();

            Assert.True(RandomIntPairs(32768).Select(tuple => def.Compare(tuple.Item1, tuple.Item2) + rev.Compare(tuple.Item1, tuple.Item2) == 0).All());
            Assert.True(RandomSameIntPairs(32768).Select(tuple => def.Compare(tuple.Item1, tuple.Item2) + rev.Compare(tuple.Item1, tuple.Item2) == 0).All());
        }

        [Fact]
        public void ComparerNullParamTest() {
            Assert.Throws<ArgumentNullException>(() => {
                var c = new LambdaComparer<int>((Func<int, int, int>)null);
            });

            Assert.Throws<ArgumentNullException>(() => {
                var c = new LambdaComparer<int>((Func<int, int, bool>)null);
            });

            Assert.Throws<ArgumentNullException>(() => {
                var c = new ReverseComparer<int>(null);
            });
        }
    }
}
