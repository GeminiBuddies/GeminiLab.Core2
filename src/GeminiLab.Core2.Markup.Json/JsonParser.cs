using System;
using System.Collections.Generic;
using System.IO;
using GeminiLab.Core2.Text;

namespace GeminiLab.Core2.Markup.Json {
    public static class JsonParser {
        private static JsonToken readToken(JsonTokenizer tokenizer) {
            if (tokenizer.GetToken(out var token) != JsonGetTokenError.NoError) throw new Exception();
            return token;
        }

        private static JsonValue parseJsonValue(JsonTokenizer tokenizer, JsonToken top) {
            return top.Type switch {
                JsonTokenType.LBrace => (JsonValue)parseJsonObject(tokenizer, top),
                JsonTokenType.LBracket => parseJsonArray(tokenizer, top),
                JsonTokenType.LiteralTrue => new JsonBool(true),
                JsonTokenType.LiteralFalse => new JsonBool(false),
                JsonTokenType.LiteralNull => new JsonNull(),
                JsonTokenType.String => new JsonString(EscapeSequenceConverter.Decode(top.Value)),
                JsonTokenType.Number => new JsonNumber(top.Value.ToString()),
                /*
                case JsonTokenType.RBracket:
                case JsonTokenType.RBrace:
                case JsonTokenType.Colon:
                case JsonTokenType.Comma:
                case JsonTokenType.NotAToken:
                case anything else:
                */
                _ => throw new Exception(),
            };
        }

        private static JsonObject parseJsonObject(JsonTokenizer tokenizer, JsonToken top) {
            var cache = new List<JsonObjectKeyValuePair>();

            var token = readToken(tokenizer);
            if (token.Type == JsonTokenType.RBrace) {
                return new JsonObject(cache);
            }

            while (true) {
                var key = parseJsonString(tokenizer, token);

                token = readToken(tokenizer);
                if (token.Type != JsonTokenType.Colon) throw new Exception();

                token = readToken(tokenizer);
                var value = parseJsonValue(tokenizer, token);

                cache.Add(new JsonObjectKeyValuePair(key, value));

                token = readToken(tokenizer);
                if (token.Type == JsonTokenType.Comma) {
                    token = readToken(tokenizer);
                    continue;
                }
                if (token.Type == JsonTokenType.RBrace) break;
                throw new Exception();
            }

            return new JsonObject(cache);
        }

        private static JsonArray parseJsonArray(JsonTokenizer tokenizer, JsonToken top) {
            var cache = new List<JsonValue>();

            var token = readToken(tokenizer);
            if (token.Type == JsonTokenType.RBracket) {
                return new JsonArray(cache);
            }

            while (true) {
                cache.Add(parseJsonValue(tokenizer, token));

                token = readToken(tokenizer);
                if (token.Type == JsonTokenType.Comma) {
                    token = readToken(tokenizer);
                    continue;
                }
                if (token.Type == JsonTokenType.RBracket) break;

                throw new Exception();
            }

            return new JsonArray(cache);
        }

        private static JsonString parseJsonString(JsonTokenizer tokenizer, JsonToken top) {
            if (top.Type != JsonTokenType.String) throw new Exception();
            return new JsonString(EscapeSequenceConverter.Decode(top.Value));
        }

        public static JsonValue Parse(string value) {
            var tokenizer = new JsonTokenizer(new StringReader(value));
            if (tokenizer.GetToken(out var token) != JsonGetTokenError.NoError) throw new Exception();

            return parseJsonValue(tokenizer, token);
        }
    }
}