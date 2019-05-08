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
            var yielder = Yielder.Repeat(7);

            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());

            int x = 1;
            yielder = Yielder.Iterate(() => x *= 2);
            Assert.Equal(2, yielder.GetNext());
            Assert.Equal(4, yielder.GetNext());
            Assert.Equal(8, yielder.GetNext());
            Assert.Equal(16, yielder.GetNext());
        }

        [Fact]
        public void YielderExtensionFilter() {
            int x = 1;
            var yielder = Yielder.Iterate(() => x += 2);
            var filtered = yielder.Filter(v => v % 3 == 0);
            Assert.Equal(3, filtered.GetNext());
            Assert.Equal(9, filtered.GetNext());
            Assert.Equal(15, filtered.GetNext());
            Assert.Equal(21, filtered.GetNext());
        }

        [Fact]
        public void YielderExtensionMap() {
            int x = 1;
            var yielder = Yielder.Iterate(() => x += 2);
            var mapped = yielder.Map(v => v % 3);
            Assert.Equal(0, mapped.GetNext());
            Assert.Equal(2, mapped.GetNext());
            Assert.Equal(1, mapped.GetNext());
            Assert.Equal(0, mapped.GetNext());
        }

        [Fact]
        public void YielderExtensionFilterAndMap() {
            int x = 1;
            var yielder = Yielder.Iterate(() => x += 2);
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
            var yielder = Yielder.Iterate(() => ((v = !v)) ? (Base)new A { Num = x++ } : new B() { Str = $"{x++}" });

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
    }
}
