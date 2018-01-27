namespace GeminiLab.Core2.ML.Json {
    public struct JsonObjectKeyValuePair {
        public JsonString Key { get; }
        public JsonValue Value { get; }

        public JsonObjectKeyValuePair(JsonString key, JsonValue value) {
            Key = key;
            Value = value;
        }
    }
}