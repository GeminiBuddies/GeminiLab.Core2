using System;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
    public sealed class JsonBool : JsonValue, IEquatable<JsonBool> {
        public bool Value { get; }

        public JsonBool(bool value) {
            Value = value;
        }

        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            iw.Write(Value ? "true" : "false");
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is JsonBool b && Equals(b);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public bool Equals(JsonBool other) {
            return other?.Value == Value;
        }

        public static bool operator ==(JsonBool a, JsonBool b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(JsonBool a, JsonBool b) => !(a == b);

        public static implicit operator JsonBool(bool val) => new JsonBool(val);
        public static implicit operator bool(JsonBool val) => val.Value;
    }
}