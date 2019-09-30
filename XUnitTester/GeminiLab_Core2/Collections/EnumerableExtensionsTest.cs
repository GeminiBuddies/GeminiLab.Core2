using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeminiLab.Core2;
using GeminiLab.Core2.Collections;
using Xunit;

namespace XUnitTester.GeminiLab_Core2.Collections {
    public static class EnumerableExtensionsTest {
        [Fact]
        public static void ForEachTest() {
            var x = 0.To(1024);
            var xl = x.ToList();
            var l = new List<int>();

            x.ForEach(l.Add);

            Assert.Equal(xl, l);
        }

        [Fact]
        public static void UnionIntersectTest() {
            var a = (-4).To(26, 3); // -4, -1, 2, 5, 8, 11, 14, 17, 20, 23
            var b = (-3).To(30, 4); // -3, 1, 5, 9, 13, 17, 21, 25, 29
            var c = 4.To(22, 2);    // 6, 8, 10, 12, 14, 16, 18, 20
            var d = 5.To(15);       // [5, 15)

            Assert.Equal(new[] { 5, 17 }, new[] { a, b }.Intersect());
            Assert.Equal(new[] { 8, 14, 20 }, new[] { a, c }.Intersect());
            Assert.Equal(Array.Empty<int>(), new[] { a, b, c }.Intersect());
            Assert.Equal(new[] { 8, 14 }, new[] { a, d, c }.Intersect());

            var l = new[] { a, b }.Union().ToList();
            l.Sort();
            Assert.Equal(new[] { -4, -3, -1, 1, 2, 5, 8, 9, 11, 13, 14, 17, 20, 21, 23, 25, 29 }, l);
            l = new[] { d, c }.Union().ToList();
            l.Sort();
            Assert.Equal(new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 18, 20 }, l);
        }

        [Fact]
        public static void FlattenTest() {
            var a = (-4).To(26, 3); // -4, -1, 2, 5, 8, 11, 14, 17, 20, 23
            var b = (-3).To(30, 4); // -3, 1, 5, 9, 13, 17, 21, 25, 29
            var c = new[] { 5, 7, 11, 19, 35 };

            Assert.Equal(new[] { -4, -1, 2, 5, 8, 11, 14, 17, 20, 23, -3, 1, 5, 9, 13, 17, 21, 25, 29, 5, 7, 11, 19, 35 }, new IEnumerable<int>[] { a, b, c }.Flatten());
        }

        [Fact]
        public static void RemoveNullTest() {
            var a = new[] { "a", null, "bb", "ccc", null, "dddd", null };

            Assert.Equal(new[] { "a", "bb", "ccc", "dddd" }, a.RemoveNull());
        }

        [Fact]
        public static void TakeTest() {
            Assert.Equal(1.To(5), 1.To(12).Take(0, 4));
            Assert.Equal(3.To(12, 2), 1.To(12, 2).Take(1, 5));
            Assert.Equal(Array.Empty<int>(), 1.To(12, 2).Take(128, 5));
            Assert.Equal(3.To(12, 2), 1.To(12, 2).Take(1, 128));
        }

        [Fact]
        public static void NumberItemsTest() {
            var x = 1.To(128, 2).NumberItems();

            for (int i = 0; i < 64; ++i) {
                Assert.True(x.TryGetValue(i, out int v));
                Assert.Equal(i * 2 + 1, v);
            }
            
            x = 1.To(128, 2).NumberItems(1);

            for (int i = 0; i < 64; ++i) {
                Assert.True(x.TryGetValue(i + 1, out int v));
                Assert.Equal(i * 2 + 1, v);
            }
        }

        [Fact]
        public static void BoolAllTest() {
            Assert.True(0.To(100, 2).Select(x => x % 2 == 0).All());
            Assert.False(0.To(100).Select(x => x < 85).All());
        }

        [Fact]
        public static void BoolAnyTest() {
            Assert.False(1.To(100, 2).Select(x => x % 2 == 0).Any());
            Assert.True(0.To(100).Select(x => x > 85).Any());
        }

        [Fact]
        public static void IllParameterTest() {
            var nil = (IEnumerable<ulong>)null;
            var arr = (IEnumerable<ulong>)new[] { 1ul, 2ul };
            var nil2 = (IEnumerable<IEnumerable<ulong>>)null;
            var nilC = (IEnumerable<IEnumerable<ulong>>)new[] { (IEnumerable<ulong>)null, new[] { 1ul } };
            var nilBool = (IEnumerable<bool>)null;

            Assert.Throws<ArgumentNullException>(() => { nil.Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil.ForEach(x => { }); });
            Assert.Throws<ArgumentNullException>(() => { arr.ForEach((Action<ulong>)null); });
            Assert.Throws<ArgumentNullException>(() => { nil2.Union().Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil2.Union(EqualityComparer<ulong>.Default).Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil2.Intersect().Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil2.Intersect(EqualityComparer<ulong>.Default).Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil2.Flatten().Consume(); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { nilC.Flatten().Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil.RemoveNull().Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil.Take(1, 2).Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nil.NumberItems().Consume(); });
            Assert.Throws<ArgumentNullException>(() => { nilBool.Any(); });
            Assert.Throws<ArgumentNullException>(() => { nilBool.All(); });
        }
    }
}
