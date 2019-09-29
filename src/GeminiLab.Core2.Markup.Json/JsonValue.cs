using System;
using System.IO;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
    public abstract class JsonValue {
        internal abstract void Stringify(JsonStringifyOption config, IndentedWriter iw);

        public string ToString(JsonStringifyOption config, string newline) {
            using var sw = new StringWriter();
            using var iw = new IndentedWriter(sw) { NewLine = newline };
            Stringify(config, iw);
            return sw.ToString();
        }

        public string ToString(JsonStringifyOption config) => ToString(config, Environment.NewLine);

        public sealed override string ToString() => ToString(JsonStringifyOption.Inline, Environment.NewLine);

        public static implicit operator JsonValue(string str) => (JsonString)str;
        public static implicit operator JsonValue(int val) => (JsonNumber)val;
        public static implicit operator JsonValue(double val) => (JsonNumber)val;
        public static implicit operator JsonValue(bool val) => (JsonBool)val;
    }
}