using System;
using System.IO;
using GeminiLab.Core2.IO;
using Xunit;

namespace XUnitTester.GeminiLab_Core2.IO {
    public class IndentedWriterTest {
        [Fact]
        public void BasicTest() {
            using var sw = new StringWriter();
            using var iw = new IndentedWriter(sw) { IndentString = "#", NewLine = "\n" };

            Assert.Equal("\n", iw.NewLine);
            Assert.Equal(sw.Encoding, iw.Encoding);

            iw.WriteLine("line1");
            iw.Write("line");
            iw.IncreaseIndent();
            iw.WriteLine("2");
            iw.Write("line");
            iw.Write('3');
            iw.WriteLine();
            iw.IncreaseIndent(2);
            iw.WriteLine("line4");
            iw.DecreaseIndent();
            iw.WriteLine("line5");
            iw.DecreaseIndent(6);
            iw.WriteLine("line6");

            Assert.Equal("line1\nline2\n#line3\n###line4\n##line5\nline6\n", sw.ToString());
        }

        [Fact]
        public void IllParameterTest() {
            Assert.Throws<ArgumentNullException>(() => new IndentedWriter(null, null));
        }
    }
}
