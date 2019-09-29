using System.IO;
using GeminiLab.Core2.Markup.Json;
using Xunit;

namespace XUnitTester.GeminiLab_Core2_Markup_Json {
    public static class TokenizerTest {
        [Fact]
        public static void Test() {
            var tok = new JsonTokenizer(new StringReader("{} \"123"));
            JsonToken token;
            Assert.Equal(JsonGetTokenError.NoError, tok.GetToken(out token));
            Assert.Equal("JsonToken { LBrace \"{\" at (1, 1) }", token.ToString());
            Assert.Equal(JsonGetTokenError.NoError, tok.GetToken(out token));
            Assert.Equal("JsonToken { RBrace \"}\" at (1, 2) }", token.ToString());
            Assert.Equal(JsonGetTokenError.UnfinishedString, tok.GetToken(out token));
            Assert.Equal("JsonToken { String \"123\" at (1, 4) }", token.ToString());
            Assert.Equal(JsonGetTokenError.EndOfInput, tok.GetToken(out token));
        }
    }
}
