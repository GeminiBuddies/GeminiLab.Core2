using System;
using System.Collections.Generic;
using System.IO;
using GeminiLab.Core2.Text;

namespace GeminiLab.Core2.Markup.Json {
    public static class JsonParser {
        private static JsonToken ReadToken(JsonTokenizer tokenizer) {
            if (tokenizer.GetToken(out var token) != JsonGetTokenError.NoError) throw new Exception();
            return token;
        }

        private static JsonValue ParseJsonValue(JsonTokenizer tokenizer, JsonToken top) {
            return top.Type switch {
                JsonTokenType.LBrace => (JsonValue)ParseJsonObject(tokenizer, top),
                JsonTokenType.LBracket => ParseJsonArray(tokenizer, top),
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

        private static JsonObject ParseJsonObject(JsonTokenizer tokenizer, JsonToken top) {
            var cache = new List<JsonObjectKeyValuePair>();

            var token = ReadToken(tokenizer);
            if (token.Type == JsonTokenType.RBrace) {
                return new JsonObject(cache);
            }

            while (true) {
                var key = ParseJsonString(tokenizer, token);

                token = ReadToken(tokenizer);
                if (token.Type != JsonTokenType.Colon) throw new Exception();

                token = ReadToken(tokenizer);
                var value = ParseJsonValue(tokenizer, token);

                cache.Add(new JsonObjectKeyValuePair(key, value));

                token = ReadToken(tokenizer);
                if (token.Type == JsonTokenType.Comma) {
                    token = ReadToken(tokenizer);
                    continue;
                }
                if (token.Type == JsonTokenType.RBrace) break;
                throw new Exception();
            }

            return new JsonObject(cache);
        }

        private static JsonArray ParseJsonArray(JsonTokenizer tokenizer, JsonToken top) {
            var cache = new List<JsonValue>();

            var token = ReadToken(tokenizer);
            if (token.Type == JsonTokenType.RBracket) {
                return new JsonArray(cache);
            }

            while (true) {
                cache.Add(ParseJsonValue(tokenizer, token));

                token = ReadToken(tokenizer);
                if (token.Type == JsonTokenType.Comma) {
                    token = ReadToken(tokenizer);
                    continue;
                }
                if (token.Type == JsonTokenType.RBracket) break;

                throw new Exception();
            }

            return new JsonArray(cache);
        }

        private static JsonString ParseJsonString(JsonTokenizer tokenizer, JsonToken top) {
            if (top.Type != JsonTokenType.String) throw new Exception();
            return new JsonString(EscapeSequenceConverter.Decode(top.Value));
        }

        public static JsonValue Parse(string value) {
            using var sr = new StringReader(value);
            var tokenizer = new JsonTokenizer(sr);
            if (tokenizer.GetToken(out var token) != JsonGetTokenError.NoError) throw new Exception();

            return ParseJsonValue(tokenizer, token);
        }
    }
}