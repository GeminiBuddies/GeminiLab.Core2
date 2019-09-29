using System;
using System.Collections.Generic;
using System.Linq;
using GeminiLab.Core2;
using Xunit;

using GeminiLab.Core2.Random;

namespace XUnitTester.GeminiLab_Core2_Random {
    public class ChooserTest {
        [Fact]
        public void MakeChooser() {
            var odds = 1.To(1048577, 2).ToList();

            for (int i = 0; i < 1024; ++i) {
                Assert.True(odds.Choose() % 2 == 1);
                Assert.InRange(odds.Choose(), 1, 1048575);
            }

            var chooser = new Chooser<int>(odds);
            for (int i = 0; i < 1024; ++i) {
                Assert.True(chooser.Next() % 2 == 1);
                Assert.InRange(chooser.Next(), 1, 1048575);
            }

            chooser = new Chooser<int>(odds, DefaultRNG.I32);
            for (int i = 0; i < 1024; ++i) {
                Assert.True(chooser.Next() % 2 == 1);
                Assert.InRange(chooser.Next(), 1, 1048575);
            }

            chooser = odds.MakeChooser();
            for (int i = 0; i < 1024; ++i) {
                Assert.True(chooser.Next() % 2 == 1);
                Assert.InRange(chooser.Next(), 1, 1048575);
            }

            chooser = odds.MakeChooser(DefaultRNG.I32);
            for (int i = 0; i < 1024; ++i) {
                Assert.True(chooser.Next() % 2 == 1);
                Assert.InRange(chooser.Next(), 1, 1048575);
            }

            Assert.Throws<ArgumentNullException>(() => odds.MakeChooser(null));
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null).MakeChooser());
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null).MakeChooser(DefaultRNG.I32));
        }
    }
}
