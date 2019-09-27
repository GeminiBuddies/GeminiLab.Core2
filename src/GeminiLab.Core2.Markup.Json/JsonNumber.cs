using System;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
    public sealed class JsonNumber : JsonValue, IEquatable<JsonNumber> {
        public bool IsFloat { get; }
        public int ValueInt { get; }
        public double ValueFloat { get; }

        public JsonNumber(int valueInt) {
            IsFloat = false;
            ValueInt = valueInt;
        }

        public JsonNumber(double valueFloat) {
            IsFloat = true;
            ValueFloat = valueFloat;
        }

        internal JsonNumber(string value) {
            if (int.TryParse(value, out int i)) {
                IsFloat = false;
                ValueInt = i;
            } else if (double.TryParse(value, out double f)) {
                IsFloat = true;
                ValueFloat = f;
            } else {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            if (IsFloat) iw.Write(ValueFloat);
            else iw.Write(ValueInt);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is JsonNumber num)) return false;
            return Equals(num);
        }

        public override int GetHashCode() {
            return IsFloat ? ValueFloat.GetHashCode() : ValueInt.GetHashCode();
        }

        public bool Equals(JsonNumber other) {
            if (other == null!) return false;
            if (other.IsFloat ^ IsFloat) return false;

            return IsFloat ? ValueFloat.Equals(other.ValueFloat) : ValueInt.Equals(other.ValueInt);
        }

        public static bool operator ==(JsonNumber a, JsonNumber b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(JsonNumber a, JsonNumber b) => !(a == b);

        public static implicit operator JsonNumber(int val) => new JsonNumber(val);
        public static implicit operator JsonNumber(double val) => new JsonNumber(val);
        public static implicit operator double(JsonNumber val) => val.IsFloat ? val.ValueFloat : val.ValueInt;
    }
}