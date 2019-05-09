using System;
using GeminiLab.Core2;
using GeminiLab.Core2.Sugar;
using Xunit;

using GeminiLab.Core2.Yielder;
using XUnitTester.DummyClasses;

namespace XUnitTester.GeminiLab_Core2_Yielder {
    public class FiniteYielderTest {
        [Fact]
        public void FiniteYielderBase() {
            var yielder = FiniteYielder.Const(7, 4);

            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.Equal(7, yielder.GetNext());
            Assert.False(yielder.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionAll() {
            Assert.True(Yielder.NaturalNumber().Map(v => v * 2 + 1).Take(2).All(v => v % 3 < 2));
            Assert.False(Yielder.NaturalNumber().Map(v => v * 2 + 1).Take(3).All(v => v % 3 < 2));
        }

        [Fact]
        public void FiniteYielderExtensionAny() {
            Assert.False(Yielder.NaturalNumber().Map(v => v * 2 + 1).Take(2).Any(v => v % 3 == 2));
            Assert.True(Yielder.NaturalNumber().Map(v => v * 2 + 1).Take(3).Any(v => v % 3 == 2));
        }

        [Fact]
        public void FiniteYielderExtensionCount() {
            Assert.Equal(3, Yielder.NaturalNumber().Map(v => v * 2 + 3).Take(4).Filter(v => v % 3 < 2).Count());
        }

        [Fact]
        public void FiniteYielderExtensionContains() {
            Assert.True(Yielder.NaturalNumber().Map(v => v * 2 + 3).Take(4).Contains(7));
            Assert.False(Yielder.NaturalNumber().Map(v => v * 2 + 3).Take(4).Contains(11));
        }

        [Fact]
        public void FiniteYielderExtensionFilter() {
            int x = 1;
            var yielder = Yielder.Repeat(() => x += 2).Take(4);
            var filtered = yielder.Filter(v => v % 3 == 0);
            Assert.Equal(3, filtered.GetNext());
            Assert.Equal(9, filtered.GetNext());
            Assert.False(filtered.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 1).Take(4);
            var mapped = yielder.Map(v => v % 3);
            Assert.Equal(1, mapped.GetNext());
            Assert.Equal(0, mapped.GetNext());
            Assert.Equal(2, mapped.GetNext());
            Assert.Equal(1, mapped.GetNext());
            Assert.False(mapped.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionFilterAndMap() {
            var yielder = Yielder.NaturalNumber().Map(v => v * 2 + 3).Take(4);
            var selected = yielder.Filter(v => v % 3 < 2).Map(v => v + 4);
            Assert.Equal(7, selected.GetNext());
            Assert.Equal(11, selected.GetNext());
            Assert.Equal(13, selected.GetNext());
            Assert.False(selected.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionOfType() {
            bool v = true;
            int x = 0;
            // ReSharper disable once AssignmentInConditionalExpression
            var yielder = Yielder.Repeat(() => ((v = !v)) ? (Base)new A { Num = x++ } : new B() { Str = $"{x++}" }).Take(8);

            var aYielder = yielder.OfType<Base, A>();
            Assert.Equal(1, aYielder.GetNext().Num);
            Assert.Equal(3, aYielder.GetNext().Num);
            Assert.Equal(5, aYielder.GetNext().Num);
            Assert.Equal(7, aYielder.GetNext().Num);
            Assert.False(aYielder.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionSkip() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v).Take(10);
            var skipped = yielder.Skip(5);

            Assert.Equal(36, skipped.GetNext());
            Assert.Equal(49, skipped.GetNext());
            Assert.Equal(64, skipped.GetNext());
            Assert.Equal(81, skipped.GetNext());
            Assert.Equal(100, skipped.GetNext());
            Assert.False(skipped.HasNext());

            yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v).Take(10);
            skipped = yielder.Skip(12);
            Assert.False(skipped.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionTake() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v).Take(6);
            var taken = yielder.Take(5);

            Assert.Equal(1, taken.GetNext());
            Assert.Equal(4, taken.GetNext());
            Assert.Equal(9, taken.GetNext());
            Assert.Equal(16, taken.GetNext());
            Assert.Equal(25, taken.GetNext());
            Assert.False(taken.HasNext());

            yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v).Take(6);
            taken = yielder.Take(7);

            Assert.Equal(1, taken.GetNext());
            Assert.Equal(4, taken.GetNext());
            Assert.Equal(9, taken.GetNext());
            Assert.Equal(16, taken.GetNext());
            Assert.Equal(25, taken.GetNext());
            Assert.Equal(36, taken.GetNext());
            Assert.False(taken.HasNext());
        }

        [Fact]
        public void FiniteYielderExtensionTakeWhile() {
            var yielder = Yielder.NaturalNumber().Skip(1).Map(v => v * v).Take(10);
            var taken = yielder.TakeWhile(v => v < 25);

            Assert.Equal(1, taken.GetNext());
            Assert.Equal(4, taken.GetNext());
            Assert.Equal(9, taken.GetNext());
            Assert.Equal(16, taken.GetNext());
            Assert.False(taken.HasNext());
        }
    }
}
