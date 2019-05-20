using System.Text;
using Xunit;

using GeminiLab.Core2;
using GeminiLab.Core2.Base64;
// ReSharper disable StringLiteralTypo

namespace XUnitTester.GeminiLab_Core2 {
    public class Base64Test {
        [Fact]
        public void Base64Base() {
            Assert.Equal("Yw==", new byte[] { 0x63 }.ToBase64());
            Assert.Equal("Yzk=", new byte[] { 0x63, 0x39 }.ToBase64());
            Assert.Equal("Yzkz", new byte[] { 0x63, 0x39, 0x33 }.ToBase64());
            Assert.Equal((byte)'c', "Yw==".AsBase64()[0]);
            Assert.Equal((byte)'9', "Yzk=".AsBase64()[1]);
            Assert.Equal((byte)'3', "Yzkz".AsBase64()[2]);
        }

        [Fact]
        public void Base64String() {
            Assert.Equal("QW96b3Jhd29rb2V0ZQ==", "Aozorawokoete".ToBase64());
            Assert.Equal("Kinouyorimo", "S2lub3V5b3JpbW8=".DecodeBase64());
            Assert.Equal("6Iuf5YWo5oCn5ZG95LqO5Lmx5LiW", "苟全性命于乱世".ToBase64(Encoding.UTF8));
            Assert.Equal("不求闻达于诸侯", "5LiN5rGC6Ze76L6+5LqO6K+45L6v".DecodeBase64(Encoding.UTF8));
            Assert.Equal("5pyI44GM44GN44KM44GE", "月がきれい".ToBase64(Encoding.UTF8));
            Assert.Equal("Das sind Wörter.", "RGFzIHNpbmQgV8O2cnRlci4=".DecodeBase64(Encoding.UTF8));
            Assert.Equal("0K8g0LjQtNGDINC90LAg0YDQsNCx0L7RgtGDLg==", "Я иду на работу.".ToBase64(Encoding.UTF8));
        }
    }
}
