using Xunit;

using GeminiLab.Core2.Random;

namespace XUnitTester.GeminiLab_Core2_Random {
    public class ChooserTest {
        private static readonly int[] Odds = {1, 3, 5, 7, 9};

        [Fact]
        public void MakeChooser() {
            var chooser = new Chooser<int>(Odds);
            Assert.True(chooser.Next() % 2 == 1);

            chooser = new Chooser<int>(Odds, DefaultRNG.I32);
            Assert.InRange(chooser.Next(), 1, 9);

            chooser = Odds.MakeChooser();
            Assert.True(chooser.Next() % 2 == 1);

            chooser = Odds.MakeChooser(DefaultRNG.I32);
            Assert.InRange(chooser.Next(), 1, 9);
        }
    }
}
