using GeminiLab.Core2.IO;
using System;

namespace GeminiLab.Core2.Markup.Json {
    public sealed class JsonNull : JsonValue, IEquatable<JsonNull> {
        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            iw.Write("null");
        }

        public bool Equals(JsonNull other) => true;

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return true;
            return obj is JsonNull;
        }

        public override int GetHashCode() => 0;

        public static bool operator ==(JsonNull a, JsonNull b) => true;
        public static bool operator !=(JsonNull a, JsonNull b) => false;
    }
}
