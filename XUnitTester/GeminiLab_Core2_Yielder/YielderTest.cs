using Xunit;

using GeminiLab.Core2.Yielder;
using XUnitTester.DummyClasses;

namespace XUnitTester.GeminiLab_Core2_Yielder {
    public class YielderTest {
        [Fact]
        public void YielderBase() {
            var yielder = Yielder.Const(7);

            Assert.Equal(7, yielder.Next());
            Assert.Equal(7, yielder.Next());
            Assert.Equal(7, yielder.Next());
            Assert.Equal(7, yielder.Next());

            int x = 1;
            yielder = Yielder.Repeat(() => x *= 2);
            Assert.Equal(2, yielder.Next());
            Assert.Equal(4, yielder.Next());
            Assert.Equal(8, yielder.Next());
            Assert.Equal(16, yielder.Next());

            yielder = Yielder.Iterate(v => v * 3, 1);
            Assert.Equal(3, yielder.Next());
            Assert.Equal(9, yielder.Next());
            Assert.Equal(27, yielder.Next());
            Assert.Equal(81, yielder.Next());
        }

        [Fact]
        public void YielderExtensionFilter() {
            int x = 1;
            var yielder = Yielder.Repeat(() => x += 2);
            var filtered = yielder.Filter(v => v % 3 == 0);
            Assert.Equal(3, filtered.Next());
            Assert.Equal(9, filtered.Next());
            Assert.Equal(15, filtered.Next());
            Assert.Equal(21, filtered.Next());
        }

        [Fact]
        public void YielderExtensionMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 3);
            var mapped = yielder.Map(v => v % 3);
            Assert.Equal(0, mapped.Next());
            Assert.Equal(2, mapped.Next());
            Assert.Equal(1, mapped.Next());
            Assert.Equal(0, mapped.Next());
        }

        [Fact]
        public void YielderExtensionFilterAndMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 3);
            var selected = yielder.Filter(v => v % 3 < 2).Map(v => v + 4);
            Assert.Equal(7, selected.Next());
            Assert.Equal(11, selected.Next());
            Assert.Equal(13, selected.Next());
            Assert.Equal(17, selected.Next());
        }

        [Fact]
        public void YielderExtensionOfType() {
            bool v = true;
            int x = 0;
            // ReSharper disable once AssignmentInConditionalExpression
            var yielder = Yielder.Repeat(() => ((v = !v)) ? (Base)new A { Num = x++ } : new B() { Str = $"{x++}" });

            var aYielder = yielder.OfType<Base, A>();
            Assert.Equal(1, aYielder.Next().Num);
            Assert.Equal(3, aYielder.Next().Num);
            Assert.Equal(5, aYielder.Next().Num);
            Assert.Equal(7, aYielder.Next().Num);

            var bYielder = yielder.OfType<Base, B>();
            Assert.Equal("8", bYielder.Next().Str);
            Assert.Equal("10", bYielder.Next().Str);
            Assert.Equal("12", bYielder.Next().Str);
            Assert.Equal("14", bYielder.Next().Str);
        }

        [Fact]
        public void YielderExtensionSkip() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var skipped = yielder.Skip(5);

            Assert.Equal(36, skipped.Next());
            Assert.Equal(49, skipped.Next());
            Assert.Equal(64, skipped.Next());
            Assert.Equal(81, skipped.Next());
            Assert.Equal(100, skipped.Next());
        }

        [Fact]
        public void YielderExtensionTake() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var taken = yielder.Take(5);

            Assert.Equal(1, taken.Next());
            Assert.Equal(4, taken.Next());
            Assert.Equal(9, taken.Next());
            Assert.Equal(16, taken.Next());
            Assert.Equal(25, taken.Next());
            Assert.False(taken.HasNext());
        }

        [Fact]
        public void YielderExtensionTakeWhile() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var taken = yielder.TakeWhile(v => v < 25);

            Assert.Equal(1, taken.Next());
            Assert.Equal(4, taken.Next());
            Assert.Equal(9, taken.Next());
            Assert.Equal(16, taken.Next());
            Assert.False(taken.HasNext());
        }

        [Fact]
        public void YielderExtensionZip() {
            var yielderA = Yielder.NaturalNumber().Filter(x => x % 2 == 0);
            var yielderB = Yielder.NaturalNumber().Filter(x => x % 2 != 0);

            var zipped = yielderA.Zip(yielderB, (a, b) => a + b);

            Assert.Equal(1, zipped.Next());
            Assert.Equal(5, zipped.Next());
            Assert.Equal(9, zipped.Next());
            Assert.Equal(13, zipped.Next());

            zipped = yielderA.Zip(yielderB, (a, b) => a - b);

            Assert.Equal(-1, zipped.Next());
            Assert.Equal(-1, zipped.Next());
            Assert.Equal(-1, zipped.Next());
            Assert.Equal(-1, zipped.Next());
        }
    }
}
