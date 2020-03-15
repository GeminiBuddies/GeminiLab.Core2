using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
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

        public bool TryGetValue(string key, [MaybeNullWhen(false)]out JsonValue value) {
            value = null!;
            if (!ContainsKey(key)) return false;

            value = this[key];
            return true;
        }

        public bool TryGetJsonString(string key, [MaybeNullWhen(false)]out JsonString result) {
            if (TryGetValue(key, out var value) && value is JsonString s) {
                result = s;
                return true;
            } else {
                result = null!;
                return false;
            }
        }

        public bool TryGetJsonObject(string key, [MaybeNullWhen(false)]out JsonObject result) {
            if (TryGetValue(key, out var value) && value is JsonObject o) {
                result = o;
                return true;
            } else {
                result = null!;
                return false;
            }
        }

        public bool TryGetJsonArray(string key, [MaybeNullWhen(false)]out JsonArray result) {
            if (TryGetValue(key, out var value) && value is JsonArray arr) {
                result = arr;
                return true;
            } else {
                result = null!;
                return false;
            }
        }

        public bool TryGetJsonNumber(string key, [MaybeNullWhen(false)]out JsonNumber result) {
            if (TryGetValue(key, out var value) && value is JsonNumber number) {
                result = number;
                return true;
            } else {
                result = null!;
                return false;
            }
        }
        
        public bool TryGetJsonBool(string key, [MaybeNullWhen(false)]out JsonBool result) {
            if (TryGetValue(key, out var value) && value is JsonBool b) {
                result = b;
                return true;
            } else {
                result = null!;
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

        public int Count => _values.Count;

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

        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            if (!Values.Any()) {
                iw.Write("{}");
                return;
            }

            bool singleLine = config.HasFlag(JsonStringifyOption.Inline);
            bool compact = config.HasFlag(JsonStringifyOption.Compact);

            if (singleLine) {
                iw.Write('{');
                if (!compact) iw.Write(' ');
            } else {
                iw.WriteLine('{');
                iw.IncreaseIndent();
            }

            bool first = true;
            foreach (var (k, v) in Values) {
                if (first) {
                    first = false;
                } else {
                    if (singleLine) {
                        iw.Write(',');
                        if (!compact) iw.Write(' ');
                    } else {
                        iw.WriteLine(',');
                    }
                }

                k.Stringify(config, iw);
                iw.Write(':');
                if (!compact) iw.Write(' ');
                v.Stringify(config, iw);
            }

            if (singleLine) {
                if (!compact) iw.Write(' ');
                iw.Write('}');
            } else {
                iw.WriteLine();
                iw.DecreaseIndent();
                iw.Write('}');
            }
        }

        // key comparer
        public int Compare(string x, string y) {
            int xv = _keyOrder.ContainsKey(x ?? throw new ArgumentNullException(nameof(x))) ? _keyOrder[x] : -1;
            int yv = _keyOrder.ContainsKey(y ?? throw new ArgumentNullException(nameof(y))) ? _keyOrder[y] : -1;

            return xv.CompareTo(yv);
        }
    }
}