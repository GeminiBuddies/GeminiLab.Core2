using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GeminiLab.Core2.Collections;

namespace GeminiLab.Core2.ML.Json {
    public abstract class JsonValue {
        /// <summary>output the minimized string.</summary>
        public abstract override string ToString();
    }

    public struct JsonObjectKeyValuePair {
        public JsonString Key { get; }
        public JsonValue Value { get; }

        public JsonObjectKeyValuePair(JsonString key, JsonValue value) {
            Key = key;
            Value = value;
        }
    }

    public sealed class JsonObject : JsonValue {
        public IEnumerable<JsonObjectKeyValuePair> Values { get; }
        
        // some (and clr) don't like tuples, make them happy
        public JsonObject(IEnumerable<JsonObjectKeyValuePair> values) {
            Values = values;
        }

        public override string ToString() => "{" + Values.Select(val => val.Key.ToString() + ":" + val.Value.ToString()).JoinBy(",") + "}";
    }

    public sealed class JsonArray : JsonValue {
        public IEnumerable<JsonValue> Values { get; }

        public JsonArray(IEnumerable<JsonValue> values) {
            Values = values;
        }

        public override string ToString() => "[" + Values.Select(val => val.ToString()).JoinBy(",") + "]";
    }

    public sealed class JsonString : JsonValue {
        public string Value { get; }

        public JsonString(string value) {
            Value = value;
        }

        public override string ToString() => "\"" + Value + "\"";
    }

    public sealed class JsonNumber : JsonValue {
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
            if (value.Contains(".")) {
                IsFloat = true;
                ValueFloat = double.Parse(value);
            } else {
                IsFloat = false;
                ValueInt = int.Parse(value);
            }
        }

        public override string ToString() => IsFloat ? ValueFloat.ToString() : ValueInt.ToString();
    }

    public sealed class JsonBool : JsonValue {
        public bool Value { get; }

        public JsonBool(bool value) {
            Value = value;
        }

        public override string ToString() => Value ? "true" : "false";
    }

    public sealed class JsonNull : JsonValue {
        public override string ToString() => "null";
    }
}
