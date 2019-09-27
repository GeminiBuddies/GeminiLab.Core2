using GeminiLab.Core2.Text;
using Xunit;

namespace XUnitTester.GeminiLab_Core2.Text {
    public class EscapeSequenceConverterText {
        [Fact]
        public void DecodeBase() {
            Assert.Equal("\b\r\t\n\0\f\"\\", EscapeSequenceConverter.Decode(@"\b\r\t\n\0\f\""\\"));
            Assert.Equal("\\\\b\\r\\t\\n\\0\\f\\\"", EscapeSequenceConverter.Decode(@"\\\\b\\r\\t\\n\\0\\f\\"""));
        }

        [Fact]
        public void DecodeUnicode() {
            Assert.Equal("スgokuかわいい", EscapeSequenceConverter.Decode(@"\u30b9gok\u0075かわい\u3044"));
        }

        [Fact]
        public void DecodeResume() {
            Assert.Equal("ara\r\\g\\", EscapeSequenceConverter.Decode(@"ara\r\g\"));
            Assert.Equal("\\ufool\\u433", EscapeSequenceConverter.Decode(@"\ufool\u433"));
        }

        [Fact]
        public void EncodeBase() {
            Assert.Equal(@"\b\r\t\n\0\f\""\\", EscapeSequenceConverter.Encode("\b\r\t\n\0\f\"\\"));
            Assert.Equal(@"\\\\b\\r\\t\\n\\0\\f\\\""ето", EscapeSequenceConverter.Encode("\\\\b\\r\\t\\n\\0\\f\\\"ето"));
        }

        [Fact]
        public void EncodeUnicode() {
            Assert.Equal(@"\b\r\t\n\0\f\""\\", EscapeSequenceConverter.EncodeToAscii("\b\r\t\n\0\f\"\\"));
            Assert.Equal(@"\\\\b\\r\\t\\n\\0\\f\\\""\u0435\u0442\u043e", EscapeSequenceConverter.EncodeToAscii("\\\\b\\r\\t\\n\\0\\f\\\"ето"));
        }
    }
}
