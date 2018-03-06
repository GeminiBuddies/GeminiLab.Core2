using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeminiLab.Core2.ML.Json {
    public abstract class JsonValue {
        internal abstract void Stringify(JsonStringifyConfig config, int indent, StringBuilder target);

        internal static void MakeIndent(StringBuilder sb, int cnt) => sb.Append(' ', cnt * 4);

        public sealed override string ToString() {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.SingleLine, 0, sb);
            return sb.ToString();
        }

        public string ToStringPrettified() {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.Prettified, 0, sb);
            sb.Append('\n');
            return sb.ToString();
        }

        public string ToStringMinimized() {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.Minimized, 0, sb);
            return sb.ToString();
        }

        public string ToStringForNetwork() {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.Network, 0, sb);
            return sb.ToString();
        }
    }

    public sealed class JsonObject : JsonValue {
        private struct JsonObjectKeyComparer : IComparer<string> {
            public JsonObject Father;

            public int Compare(string x, string y) {
                int xv = Father._keyOrder.ContainsKey(x) ? Father._keyOrder[x] : -1;
                int yv = Father._keyOrder.ContainsKey(y) ? Father._keyOrder[y] : -1;

                return xv.CompareTo(yv);
            }
        }

        private readonly Dictionary<string, int> _keyOrder;
        private readonly SortedDictionary<string, JsonValue> _values;

        public IEnumerable<JsonObjectKeyValuePair> Values {
            get {
                foreach (var i in _values) yield return new JsonObjectKeyValuePair(new JsonString(i.Key), i.Value);
            }
        }

        public JsonValue this[JsonString str] {
            get => this[str?.Value ?? throw new ArgumentNullException(nameof(str))];
            set => this[str?.Value ?? throw new ArgumentNullException(nameof(str))] = value;
        }

        public JsonValue this[string str] {
            get {
                if (str == null) throw new ArgumentNullException(nameof(str));
                if (!_values.ContainsKey(str)) throw new KeyNotFoundException();

                return _values[str];
            }
            set {
                if (str == null) throw new ArgumentNullException(nameof(str));
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (!_values.ContainsKey(str)) {
                    _keyOrder[str] = _keyOrder.Count;
                }

                _values[str] = value;
            }
        }

        public void Append(JsonString key, JsonValue value) {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (_values.ContainsKey(key.Value)) throw new ArgumentOutOfRangeException(nameof(key));
            this[key] = value;
        }

        public void Remove(JsonString key) {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (!_values.ContainsKey(key.Value)) throw new KeyNotFoundException();
            _keyOrder[key.Value] = -1;
            _values.Remove(key.Value);
        }

        public JsonObject() {
            _keyOrder = new Dictionary<string, int>();
            _values = new SortedDictionary<string, JsonValue>(new JsonObjectKeyComparer { Father = this });
        }

        public JsonObject(IEnumerable<JsonObjectKeyValuePair> values) {
            _keyOrder = new Dictionary<string, int>();
            _values = new SortedDictionary<string, JsonValue>(new JsonObjectKeyComparer { Father = this });

            foreach (var i in values) {
                Append(i.Key, i.Value);
            }
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            if (!Values.Any()) {
                target.Append("{}");
                return;
            }

            bool compact = config.Contains(JsonStringifyConfig.Compact);
            bool expandObjects = config.Contains(JsonStringifyConfig.ExpandObjects);

            string head = compact ? "{" : "{ ";
            string seperator = compact ? "," : ", ";
            string colon = compact ? ":" : ": ";
            string tail = compact ? "}" : " }";

            target.Append(expandObjects ? "{\n" : head);

            bool first = true;
            foreach (var i in Values) {
                if (!first) {
                    target.Append(expandObjects ? ",\n" : seperator);
                } else {
                    first = false;
                }

                if (expandObjects) MakeIndent(target, indent + 1);

                i.Key.Stringify(config, indent, target);
                target.Append(colon);
                i.Value.Stringify(config, indent + 1, target);
            }

            if (expandObjects) {
                target.Append('\n');
                MakeIndent(target, indent);
                target.Append('}');
            } else {
                target.Append(tail);
            }
        }
    }

    public sealed class JsonArray : JsonValue {
        public List<JsonValue> Values { get; }

        public JsonArray() {
            Values = new List<JsonValue>();
        }

        public JsonArray(IEnumerable<JsonValue> values) {
            Values = new List<JsonValue>(values);
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            if (!Values.Any()) {
                target.Append("[]");
                return;
            }

            bool compact = config.Contains(JsonStringifyConfig.Compact);
            bool expandObjects = config.Contains(JsonStringifyConfig.ExpandObjects);

            bool expandIt = expandObjects && Values.Any(x => x is JsonObject);

            string head = compact ? "[" : "[ ";
            string seperator = compact ? "," : ", ";
            string tail = compact ? "]" : " ]";

            target.Append(expandIt ? "[\n" : head);

            bool first = true;
            foreach (var i in Values) {
                if (first) first = false; else target.Append(expandIt ? ",\n" : seperator);

                if (expandIt) MakeIndent(target, indent + 1);
                i.Stringify(config, indent + 1, target);
            }

            if (expandIt) {
                target.Append('\n');
                MakeIndent(target, indent);
                target.Append(']');
            } else {
                target.Append(tail);
            }
        }
    }

    public sealed class JsonString : JsonValue {
        public string Value { get; }

        public JsonString(string value) {
            Value = value;
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append('\"');
            target.Append(config.Contains(JsonStringifyConfig.AsciiOnly) ? JsonEscapeCharsConverter.EncodeToAscii(Value) : JsonEscapeCharsConverter.Encode(Value));
            target.Append('\"');
        }
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
            if (int.TryParse(value, out int vint)) {
                IsFloat = false;
                ValueInt = vint;
            } else if (double.TryParse(value, out double vfloat)) {
                IsFloat = true;
                ValueFloat = vfloat;
            } else {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            if (IsFloat) target.Append(ValueFloat);
            else target.Append(ValueInt);
        }
    }

    public sealed class JsonBool : JsonValue {
        public bool Value { get; }

        public JsonBool(bool value) {
            Value = value;
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append(Value ? "true" : "false");
        }
    }

    public sealed class JsonNull : JsonValue {
        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append("null");
        }
    }
}
