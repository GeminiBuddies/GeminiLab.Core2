using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeminiLab.Core2.ML.Json {
    public struct JsonStringifyConfig {
        public bool Spaces;
        public bool ExpandObjects;

        public JsonStringifyConfig(bool spaces, bool expandObjects) {
            Spaces = spaces;
            ExpandObjects = expandObjects;
        }

        public static JsonStringifyConfig Minimized { get; } = new JsonStringifyConfig(false, false);
        public static JsonStringifyConfig SingleLine { get; } = new JsonStringifyConfig(true, false);
        public static JsonStringifyConfig Prettified { get; } = new JsonStringifyConfig(true, true);
    }

    public abstract class JsonValue {
        internal abstract void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target);

        internal static void MakeIndent(StringBuilder sb, int cnt) => sb.Append(' ', cnt * 4);

        public sealed override string ToString() => ToString(false);

        public string ToString(bool asciiOnly) {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.SingleLine, asciiOnly, 0, sb);
            return sb.ToString();
        }

        public string ToStringPrettified(bool asciiOnly) {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.Prettified, asciiOnly, 0, sb);
            sb.Append('\n');
            return sb.ToString();
        }

        public string ToStringMinimized(bool asciiOnly) {
            var sb = new StringBuilder();

            Stringify(JsonStringifyConfig.Minimized, asciiOnly, 0, sb);
            return sb.ToString();
        }
    }

    public sealed class JsonObject : JsonValue {
        public IEnumerable<JsonObjectKeyValuePair> Values { get; }

        // some (and clr) don't like tuples, make them happy
        public JsonObject(IEnumerable<JsonObjectKeyValuePair> values) {
            Values = values;
        }

        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            if (!Values.Any()) {
                target.Append("{}");
                return;
            }
            
            string head = config.Spaces ? "{ " : "{";
            string seperator = config.Spaces ? ", " : ",";
            string colon = config.Spaces ? ": " : ":";
            string tail = config.Spaces ? " }" : "}";

            target.Append(config.ExpandObjects ? "{\n" : head);
            
            bool first = true;
            foreach (var i in Values) {
                if (!first) {
                    target.Append(config.ExpandObjects ? ",\n" : seperator);
                } else {
                    first = false;
                }

                if (config.ExpandObjects) MakeIndent(target, indent + 1);

                i.Key.Stringify(config, asciiOnly, indent, target);
                target.Append(colon);
                i.Value.Stringify(config, asciiOnly, indent + 1, target);
            }

            if (config.ExpandObjects) {
                target.Append('\n');
                MakeIndent(target, indent);
                target.Append('}');
            } else {
                target.Append(tail);
            }
        }
    }

    public sealed class JsonArray : JsonValue {
        public IEnumerable<JsonValue> Values { get; }

        public JsonArray(IEnumerable<JsonValue> values) {
            Values = values;
        }

        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            if (!Values.Any()) {
                target.Append("[]");
                return;
            }

            bool expandIt = config.ExpandObjects && Values.Any(x => x is JsonObject);

            string head = config.Spaces ? "[ " : "[";
            string seperator = config.Spaces ? ", " : ",";
            string tail = config.Spaces ? " ]" : "]";

            target.Append(expandIt ? "[\n" : head);

            bool first = true;
            foreach (var i in Values) {
                if (first) first = false; else target.Append(expandIt ? ",\n" : seperator);

                if (expandIt) MakeIndent(target, indent + 1);
                i.Stringify(config, asciiOnly, indent + 1, target);
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

        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            target.Append('\"');
            target.Append(asciiOnly ? JsonEscapeCharsConverter.EncodeToAscii(Value) : JsonEscapeCharsConverter.Encode(Value));
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
            } else {
                IsFloat = true;
                ValueFloat = double.Parse(value);
            }
        }

        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            if (IsFloat) target.Append(ValueFloat);
            else target.Append(ValueInt);
        }
    }

    public sealed class JsonBool : JsonValue {
        public bool Value { get; }

        public JsonBool(bool value) {
            Value = value;
        }

        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            target.Append(Value ? "true" : "false");
        }
    }

    public sealed class JsonNull : JsonValue {
        internal override void Stringify(in JsonStringifyConfig config, bool asciiOnly, int indent, StringBuilder target) {
            target.Append("null");
        }
    }
}
