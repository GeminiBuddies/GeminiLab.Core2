using System;
using GeminiLab.Core2;
using GeminiLab.Core2.Sugar;
using Xunit;

using GeminiLab.Core2.Yielder;
using XUnitTester.DummyClasses;

namespace XUnitTester.GeminiLab_Core2_Yielder {
    public class YielderTest {
        [Fact]
        public void YielderBase() {
            var yielder = Yielder.Const(7);

            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());

            int x = 1;
            yielder = Yielder.Repeat(() => x *= 2);
            Assert.Equal(2, yielder.GetNext());
            Assert.Equal(4, yielder.GetNext());
            Assert.Equal(8, yielder.GetNext());
            Assert.Equal(16, yielder.GetNext());

            yielder = Yielder.Iterate(v => v * 3, 1);
            Assert.Equal(3, yielder.GetNext());
            Assert.Equal(9, yielder.GetNext());
            Assert.Equal(27, yielder.GetNext());
            Assert.Equal(81, yielder.GetNext());
        }

        [Fact]
        public void YielderExtensionFilter() {
            int x = 1;
            var yielder = Yielder.Repeat(() => x += 2);
            var filtered = yielder.Filter(v => v % 3 == 0);
            Assert.Equal(3, filtered.GetNext());
            Assert.Equal(9, filtered.GetNext());
            Assert.Equal(15, filtered.GetNext());
            Assert.Equal(21, filtered.GetNext());
        }

        [Fact]
        public void YielderExtensionMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 3);
            var mapped = yielder.Map(v => v % 3);
            Assert.Equal(0, mapped.GetNext());
            Assert.Equal(2, mapped.GetNext());
            Assert.Equal(1, mapped.GetNext());
            Assert.Equal(0, mapped.GetNext());
        }

        [Fact]
        public void YielderExtensionFilterAndMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 3);
            var selected = yielder.Filter(v => v % 3 < 2).Map(v => v + 4);
            Assert.Equal(7, selected.GetNext());
            Assert.Equal(11, selected.GetNext());
            Assert.Equal(13, selected.GetNext());
            Assert.Equal(17, selected.GetNext());
        }

        [Fact]
        public void YielderExtensionOfType() {
            bool v = true;
            int x = 0;
            // ReSharper disable once AssignmentInConditionalExpression
            var yielder = Yielder.Repeat(() => ((v = !v)) ? (Base)new A { Num = x++ } : new B() { Str = $"{x++}" });

            var aYielder = yielder.OfType<Base, A>();
            Assert.Equal(1, aYielder.GetNext().Num);
            Assert.Equal(3, aYielder.GetNext().Num);
            Assert.Equal(5, aYielder.GetNext().Num);
            Assert.Equal(7, aYielder.GetNext().Num);

            var bYielder = yielder.OfType<Base, B>();
            Assert.Equal("8", bYielder.GetNext().Str);
            Assert.Equal("10", bYielder.GetNext().Str);
            Assert.Equal("12", bYielder.GetNext().Str);
            Assert.Equal("14", bYielder.GetNext().Str);
        }

        [Fact]
        public void YielderExtensionSkip() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var skipped = yielder.Skip(5);

            Assert.Equal(36, skipped.GetNext());
            Assert.Equal(49, skipped.GetNext());
            Assert.Equal(64, skipped.GetNext());
            Assert.Equal(81, skipped.GetNext());
            Assert.Equal(100, skipped.GetNext());
        }

        [Fact]
        public void YielderExtensionTake() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var taken = yielder.Take(5);

            Assert.Equal(1, taken.GetNext());
            Assert.Equal(4, taken.GetNext());
            Assert.Equal(9, taken.GetNext());
            Assert.Equal(16, taken.GetNext());
            Assert.Equal(25, taken.GetNext());
            Assert.False(taken.HasNext());
        }

        [Fact]
        public void YielderExtensionTakeWhile() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v);
            var taken = yielder.TakeWhile(v => v < 25);

            Assert.Equal(1, taken.GetNext());
            Assert.Equal(4, taken.GetNext());
            Assert.Equal(9, taken.GetNext());
            Assert.Equal(16, taken.GetNext());
            Assert.False(taken.HasNext());
        }

        [Fact]
        public void YielderExtensionZip() {
            var yielderA = Yielder.NaturalNumber().Filter(x => x % 2 == 0);
            var yielderB = Yielder.NaturalNumber().Filter(x => x % 2 != 0);

            var zipped = yielderA.Zip(yielderB, (a, b) => a + b);

            Assert.Equal(1, zipped.GetNext());
            Assert.Equal(5, zipped.GetNext());
            Assert.Equal(9, zipped.GetNext());
            Assert.Equal(13, zipped.GetNext());

            zipped = yielderA.Zip(yielderB, (a, b) => a - b);

            Assert.Equal(-1, zipped.GetNext());
            Assert.Equal(-1, zipped.GetNext());
            Assert.Equal(-1, zipped.GetNext());
            Assert.Equal(-1, zipped.GetNext());
        }
    }
}
