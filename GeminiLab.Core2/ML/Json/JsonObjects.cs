using System;
using System.Collections;
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

        public string Stringify(JsonStringifyConfig config) {
            var sb = new StringBuilder();
            Stringify(config, 0, sb);
            return sb.ToString();
        }

        public static implicit operator JsonValue(string str) => (JsonString)str;
        public static implicit operator JsonValue(int val) => (JsonNumber)val;
        public static implicit operator JsonValue(double val) => (JsonNumber)val;
        public static implicit operator JsonValue(bool val) => (JsonBool)val;
    }

    // implement IDictionary<,> ? noway
    public sealed class JsonObject : JsonValue, IComparer<string> {
        private readonly Dictionary<string, int> _keyOrder;
        private readonly SortedDictionary<string, JsonValue> _values;

        public IEnumerable<JsonObjectKeyValuePair> Values {
            get {
                foreach (var i in _values) yield return new JsonObjectKeyValuePair(new JsonString(i.Key), i.Value);
            }
        }

        private string unpackJsonString(JsonString str) {
            return str?.Value ?? throw new ArgumentNullException();
        }

        public JsonValue this[JsonString str] {
            get => this[unpackJsonString(str)];
            set => this[unpackJsonString(str)] = value;
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
            Append(unpackJsonString(key), value);
        }

        public void Append(string key, JsonValue value) {
            if (_values.ContainsKey(key ?? throw new ArgumentNullException(nameof(key)))) throw new ArgumentOutOfRangeException(nameof(key));

            this[key] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Remove(JsonString key) {
            Remove(unpackJsonString(key));
        }

        public void Remove(string key) {
            if (!TryRemove(key)) throw new KeyNotFoundException();
        }

        public bool TryRemove(JsonString key) {
            return TryRemove(unpackJsonString(key));
        }

        public bool TryRemove(string key) {
            if (!_values.ContainsKey(key ?? throw new ArgumentNullException(nameof(key)))) return false;

            _keyOrder[key] = -1;
            _values.Remove(key);
            return true;
        }

        public bool ContainsKey(string key) {
            return _values.ContainsKey(key ?? throw new ArgumentNullException(nameof(key)));
        }

        public bool TryGetValue(string key, out JsonValue value) {
            value = null;
            if (!ContainsKey(key)) return false;

            value = this[key];
            return true;
        }

        public bool TryGetJsonString(string key, out JsonString result) {
            if (TryGetValue(key, out var value) && value is JsonString s) {
                result = s;
                return true;
            } else {
                result = null;
                return false;
            }
        }

        public bool TryGetJsonObject(string key, out JsonObject result) {
            if (TryGetValue(key, out var value) && value is JsonObject o) {
                result = o;
                return true;
            } else {
                result = null;
                return false;
            }
        }

        public bool TryGetJsonArray(string key, out JsonArray result) {
            if (TryGetValue(key, out var value) && value is JsonArray arr) {
                result = arr;
                return true;
            } else {
                result = null;
                return false;
            }
        }

        public bool TryGetJsonNumber(string key, out JsonNumber result) {
            if (TryGetValue(key, out var value) && value is JsonNumber number) {
                result = number;
                return true;
            } else {
                result = null;
                return false;
            }
        }
        
        public bool TryGetJsonBool(string key, out JsonBool result) {
            if (TryGetValue(key, out var value) && value is JsonBool b) {
                result = b;
                return true;
            } else {
                result = null;
                return false;
            }
        }

        public bool TryReadInt(string key, out int num) {
            num = 0;
            if (!TryGetValue(key, out var value)) return false;

            if (value is JsonNumber number && !number.IsFloat) {
                num = number.ValueInt;
                return true;
            }

            if (value is JsonString str && int.TryParse(str, out var result)) {
                num = result;
                return true;
            }

            return false;
        }

        public JsonObject() {
            _keyOrder = new Dictionary<string, int>();
            _values = new SortedDictionary<string, JsonValue>(this);
        }

        public JsonObject(IEnumerable<JsonObjectKeyValuePair> values) {
            _keyOrder = new Dictionary<string, int>();
            _values = new SortedDictionary<string, JsonValue>(this);

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

        // key comparer
        public int Compare(string x, string y) {
            int xv = _keyOrder.ContainsKey(x ?? throw new ArgumentNullException(nameof(x))) ? _keyOrder[x] : -1;
            int yv = _keyOrder.ContainsKey(y ?? throw new ArgumentNullException(nameof(y))) ? _keyOrder[y] : -1;

            return xv.CompareTo(yv);
        }
    }

    public sealed class JsonArray : JsonValue, IList<JsonValue> {
        public List<JsonValue> Values { get; }

        public JsonArray() {
            Values = new List<JsonValue>();
        }

        public JsonArray(IEnumerable<JsonValue> values) {
            Values = new List<JsonValue>(values);
        }

        #region interface implemention
        //IEnumerable
        public IEnumerator<JsonValue> GetEnumerator() => Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

        // ICollectiom
        public void Add(JsonValue item) => Values.Add(item);
        public void Clear() => Values.Clear();
        public bool Contains(JsonValue item) => Values.Contains(item);
        public void CopyTo(JsonValue[] array, int arrayIndex) => Values.CopyTo(array, arrayIndex);
        public bool Remove(JsonValue item) => Values.Remove(item);
        public int Count => Values.Count;
        public bool IsReadOnly => false;

        // IList
        public int IndexOf(JsonValue item) => Values.IndexOf(item);
        public void Insert(int index, JsonValue item) => Values.Insert(index, item);
        public void RemoveAt(int index) => Values.RemoveAt(index);
        public JsonValue this[int index] {
            get => Values[index];
            set => Values[index] = value;
        }
        #endregion

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

    public sealed class JsonString : JsonValue, IEquatable<JsonString> {
        public string Value { get; }

        public JsonString(string value) {
            Value = value;
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append('\"');
            target.Append(config.Contains(JsonStringifyConfig.AsciiOnly) ? JsonEscapeCharsConverter.EncodeToAscii(Value) : JsonEscapeCharsConverter.Encode(Value));
            target.Append('\"');
        }

        public bool Equals(JsonString other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is JsonString s && Equals(s);
        }

        public override int GetHashCode() {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(JsonString a, JsonString b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(JsonString a, JsonString b) => !(a == b);

        public static implicit operator string(JsonString str) => str.Value;
        public static implicit operator JsonString(string str) => new JsonString(str);
    }

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
            if (other == null) return false;
            if (other.IsFloat ^ IsFloat) return false;

            return IsFloat ? ValueFloat.Equals(other.ValueFloat) : ValueInt.Equals(other.ValueInt);
        }

        public static bool operator ==(JsonNumber a, JsonNumber b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(JsonNumber a, JsonNumber b) => !(a == b);

        public static implicit operator JsonNumber(int val) => new JsonNumber(val);
        public static implicit operator JsonNumber(double val) => new JsonNumber(val);
        public static implicit operator double(JsonNumber val) => val.IsFloat ? val.ValueFloat : val.ValueInt;
    }

    public sealed class JsonBool : JsonValue, IEquatable<JsonBool> {
        public bool Value { get; }

        public JsonBool(bool value) {
            Value = value;
        }

        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append(Value ? "true" : "false");
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

    public sealed class JsonNull : JsonValue, IEquatable<JsonNull> {
        internal override void Stringify(JsonStringifyConfig config, int indent, StringBuilder target) {
            target.Append("null");
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
