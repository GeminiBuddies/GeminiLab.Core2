using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.ML.Json {
    public static class JsonParser {
        private static JsonValue parseJsonValue(JsonTokenQueue queue) {
            var tok = queue.ReadNoErrorOrEof();

            if (tok == null) throw new JsonParsingUnexpectedEndOfFileException();

            return tok.Type switch {
                JsonTokenType.LBrace => (JsonValue)parseJsonObject(queue),
                JsonTokenType.LBracket => parseJsonArray(queue),
                JsonTokenType.LiteralTrue => new JsonBool(true),
                JsonTokenType.LiteralFalse => new JsonBool(false),
                JsonTokenType.LiteralNull => new JsonNull(),
                JsonTokenType.String => new JsonString(JsonEscapeCharsConverter.Decode(tok.Value)),
                JsonTokenType.Number => new JsonNumber(tok.Value),
                /*
                case JsonTokenType.RBracket:
                case JsonTokenType.RBrace:
                case JsonTokenType.Colon:
                case JsonTokenType.Comma:
                case JsonTokenType.Error:
                case anything else:
                */
                _ => throw new JsonParsingUnexpectedTokenException(tok),
            };
        }

        private static JsonObject parseJsonObject(JsonTokenQueue queue) {
            var cache = new List<JsonObjectKeyValuePair>();

            var tok = queue.PeekNoErrorOrEof();
            if (tok.Type == JsonTokenType.RBrace) {
                queue.Read();
                return new JsonObject(cache);
            }

            while (true) {
                var key = parseJsonString(queue);

                tok = queue.ReadNoErrorOrEof();
                if (tok.Type != JsonTokenType.Colon) throw new JsonParsingUnexpectedTokenException(tok);

                var value = parseJsonValue(queue);

                cache.Add(new JsonObjectKeyValuePair(key, value));

                tok = queue.ReadNoErrorOrEof();
                if (tok.Type == JsonTokenType.Comma) continue;
                if (tok.Type == JsonTokenType.RBrace) break;
                throw new JsonParsingUnexpectedTokenException(tok);
            }

            return new JsonObject(cache);
        }

        private static JsonArray parseJsonArray(JsonTokenQueue queue) {
            var cache = new List<JsonValue>();

            var tok = queue.PeekNoErrorOrEof();
            if (tok.Type == JsonTokenType.RBracket) {
                queue.Read();
                return new JsonArray(cache);
            }

            while (true) {

                cache.Add(parseJsonValue(queue));

                tok = queue.ReadNoErrorOrEof();
                if (tok.Type == JsonTokenType.Comma) continue;
                if (tok.Type == JsonTokenType.RBracket) break;

                throw new JsonParsingUnexpectedTokenException(tok);
            }

            return new JsonArray(cache);
        }

        private static JsonString parseJsonString(JsonTokenQueue queue) {
            var tok = queue.ReadNoErrorOrEof();
            if (tok.Type != JsonTokenType.String) throw new JsonParsingUnexpectedTokenException(tok);
            return new JsonString(JsonEscapeCharsConverter.Decode(tok.Value));
        }

        public static JsonValue Parse(string value) {
            var queue = new JsonTokenQueue(value);
            var rv = parseJsonValue(queue);

            if (queue.Peek() != null) throw new Exception(); // todo: exception class
            if (!(rv is JsonArray || rv is JsonObject)) throw new Exception(); // todo: exception class
            return rv;
        }

        public static string Prettify(string source) => Parse(source).ToStringPrettified();
        public static string Minimize(string source) => Parse(source).ToStringMinimized();
    }
}