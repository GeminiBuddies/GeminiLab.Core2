using Xunit;

using GeminiLab.Core2;

namespace XUnitTester.GeminiLab_Core2 {
    public class RangeTest {
        [Fact]
        public void RangeBase() {
            var range = new Range(2);
            var en = range.GetEnumerator();

            Assert.True(en.MoveNext());
            Assert.Equal(0, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(1, en.Current);
            Assert.False(en.MoveNext());

            en.Dispose();

            range = new Range(12, 25, 4);
            en = range.GetEnumerator();

            Assert.True(en.MoveNext());
            Assert.Equal(12, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(16, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(20, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(24, en.Current);
            Assert.False(en.MoveNext());

            en.Dispose();

            range = new Range(12, -3, -3);
            en = range.GetEnumerator();

            Assert.True(en.MoveNext());
            Assert.Equal(12, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(9, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(6, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(3, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(0, en.Current);
            Assert.False(en.MoveNext());

            en.Dispose();
        }

        [Fact]
        public void RangeInvalidParameter() {
            Assert.False(new Range(-2).GetEnumerator().MoveNext());
            Assert.False(new Range(4564, 4544).GetEnumerator().MoveNext());
            Assert.False(new Range(132, 465, -7).GetEnumerator().MoveNext());
        }

        [Fact]
        public void RangeSugar() {
            var range = 0.To(3);
            var en = range.GetEnumerator();

            Assert.True(en.MoveNext());
            Assert.Equal(0, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(1, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(2, en.Current);
            Assert.False(en.MoveNext());

            en.Dispose();

            range = 4396.To(2800, -443);
            en = range.GetEnumerator();

            Assert.True(en.MoveNext());
            Assert.Equal(4396, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(3953, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(3510, en.Current);
            Assert.True(en.MoveNext());
            Assert.Equal(3067, en.Current);
            Assert.False(en.MoveNext());

            en.Dispose();
        }

        [Fact]
        public void RangeSugarInvalidParameter() {
            Assert.False(0.To(-2).GetEnumerator().MoveNext());
            Assert.False(4564.To(4544).GetEnumerator().MoveNext());
            Assert.False(132.To(465, -7).GetEnumerator().MoveNext());
        }
    }
}
