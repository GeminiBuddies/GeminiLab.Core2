using System;
using System.Collections;
using GeminiLab.Core2.Collections;
using Xunit;

namespace XUnitTester.GeminiLab_Core2.Collections {
    public class EmptyEnumeratorTest {
        [Fact]
        public void NormalTest() {
            using var en = EmptyEnumerator<int>.Instance;

            Assert.False(en.MoveNext());
            en.Reset();
            Assert.False(en.MoveNext());
        }

        [Fact]
        public void ExceptionTest() {
            using var en = EmptyEnumerator<int>.Instance;

            Assert.Throws<InvalidOperationException>(() => {
                int x = en.Current;
            });

            Assert.Throws<InvalidOperationException>(() => {
                object x = (en as IEnumerator).Current;
            });
        }
    }
}
