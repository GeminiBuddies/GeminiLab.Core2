using System.Globalization;
using System.IO;
using System.Text;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
    public abstract class JsonValue {
        internal abstract void Stringify(JsonStringifyOption config, IndentedWriter iw);

        public string ToString(JsonStringifyOption config) {
            using var sw = new StringWriter();
            using var iw = new IndentedWriter(sw);
            Stringify(config, iw);
            return sw.ToString();
        }

        public sealed override string ToString() => ToString(JsonStringifyOption.Inline);

        public static implicit operator JsonValue(string str) => (JsonString)str;
        public static implicit operator JsonValue(int val) => (JsonNumber)val;
        public static implicit operator JsonValue(double val) => (JsonNumber)val;
        public static implicit operator JsonValue(bool val) => (JsonBool)val;
    }
}