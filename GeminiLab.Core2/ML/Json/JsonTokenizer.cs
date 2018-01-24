#define LITERAL_CASE_INSENSITIVE

using System.Collections.Generic;

namespace GeminiLab.Core2.ML.Json {
    public static partial class JsonTokenizer { }
    public sealed partial class JsonToken { }

    public enum JsonTokenType {
        LBrace, // {
        RBrace, // }
        LBracket, // [
        RBracket, // ]
        Comma, // ,
        Colon, // :
        LiteralTrue, // true
        LiteralFalse, // false
        LiteralNull, // null
        String, // string
        Number, // number
        Error
    }

    partial class JsonToken {
        public JsonTokenType Type;
        public string Value;
        public int Row;
        public int Column;

        public JsonToken(JsonTokenType type, string value, int row, int column) {
            Type = type;
            Value = value;
            Row = row;
            Column = column;
        }

        public override string ToString() {
            return nameof(JsonToken) + $" {{ {Type.ToString()} \"{Value}\" at ({Row}, {Column}) }}";
        }
    }

    partial class JsonTokenizer {
#if LITERAL_CASE_INSENSITIVE
        public static bool IsLiteralCaseInsensitive = true;
#else
        public static bool IsLiteralCaseInsensitive = false;
#endif

        // contract: a token with type "JsonTokenType.Error" is always the last token returned.
        public static IEnumerable<JsonToken> GetTokens(string src) {
            int len = src.Length;
            int r = 1, c = 1;
            bool inString = false;
            int lastpos = 0, lastc = 1;

            for (int i = 0; i <= len; ++i) {
                char curr = i == len ? '\0' : src[i];

                if (inString) {
                    if (curr == '\n' || curr == '\r' || curr == '\0') {
                        yield return new JsonToken(JsonTokenType.Error, src.Substring(lastpos, i - lastpos), r, lastc);
                        yield break;
                    }

                    if (curr == '\"') {
                        yield return new JsonToken(JsonTokenType.String, src.Substring(lastpos + 1, i - lastpos - 1), r, lastc);

                        inString = false;
                        ++c; lastc = c;
                        lastpos = i + 1;
                    } else {
                        ++c;
                    }

                } else if (curr == '[' || curr == ']' || curr == '{' || curr == '}' || curr == ',' || curr == ':'
                        || char.IsWhiteSpace(curr)
                        || curr == '\"'
                        || curr == '\0') {
                    if (lastpos < i) { // end of last token
                        var tokenStr = src.Substring(lastpos, i - lastpos);

                        // todo: maybe write a new function to do this? too ugly now
#if LITERAL_CASE_INSENSITIVE
                        if (tokenStr.ToLower() == "null") yield return new JsonToken(JsonTokenType.LiteralNull, tokenStr, r, lastc);
                        else if (tokenStr.ToLower() == "true") yield return new JsonToken(JsonTokenType.LiteralTrue, tokenStr, r, lastc);
                        else if (tokenStr.ToLower() == "false") yield return new JsonToken(JsonTokenType.LiteralFalse, tokenStr, r, lastc);
#else
                        if (tokenStr == "null") yield return new JsonToken(JsonTokenType.LiteralNull, tokenStr, r, lastc);
                        else if (tokenStr == "true") yield return new JsonToken(JsonTokenType.LiteralTrue, tokenStr, r, lastc);
                        else if (tokenStr == "false") yield return new JsonToken(JsonTokenType.LiteralFalse, tokenStr, r, lastc);
#endif
                        else if (int.TryParse(tokenStr, out int _) || double.TryParse(tokenStr, out double _)) {
                            yield return new JsonToken(JsonTokenType.Number, tokenStr, r, lastc);
                        } else {
                            yield return new JsonToken(JsonTokenType.Error, tokenStr, r, lastc);
                            yield break;
                        }
                    }

                    if (curr == '[' || curr == ']' || curr == '{' || curr == '}' || curr == ',' || curr == ':') {
                        // todo: maybe use a dictionary
                        switch (curr) {
                        case '[':
                            yield return new JsonToken(JsonTokenType.LBracket, "[", r, c); break;
                        case ']':
                            yield return new JsonToken(JsonTokenType.RBracket, "]", r, c); break;
                        case '{':
                            yield return new JsonToken(JsonTokenType.LBrace, "{", r, c); break;
                        case '}':
                            yield return new JsonToken(JsonTokenType.RBrace, "}", r, c); break;
                        case ',':
                            yield return new JsonToken(JsonTokenType.Comma, ",", r, c); break;
                        case ':':
                            yield return new JsonToken(JsonTokenType.Colon, ":", r, c); break;
                        }

                        ++c; lastc = c;
                        lastpos = i + 1;
                    } else if (curr == '\"') {
                        inString = true;
                        lastc = c; ++c;
                        lastpos = i;
                    } else if (curr == '\n' || curr == '\r') {
                        if (!(i > 0 && curr == '\n' && src[i - 1] == '\r')) {
                            ++r;
                            lastc = c = 1;
                        }

                        lastpos = i + 1;
                    } else if (curr == '\0') {
                        yield break;
                    } else {
                        ++c; lastc = c;
                        lastpos = i + 1;
                    }
                } else {
                    ++c;
                }
            }
        }
    }
}