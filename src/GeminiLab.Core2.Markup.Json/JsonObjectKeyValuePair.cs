namespace GeminiLab.Core2.Markup.Json {
    public struct JsonObjectKeyValuePair {
        public JsonString Key { get; }
        public JsonValue Value { get; }

        public JsonObjectKeyValuePair(JsonString key, JsonValue value) {
            Key = key;
            Value = value;
        }

        public void Deconstruct(out JsonString key, out JsonValue value) {
            key = Key;
            value = Value;
        }
    }
}