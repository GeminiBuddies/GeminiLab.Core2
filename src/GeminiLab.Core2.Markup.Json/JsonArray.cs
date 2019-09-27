using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeminiLab.Core2.IO;

namespace GeminiLab.Core2.Markup.Json {
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

        // ICollection
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
        
        internal override void Stringify(JsonStringifyOption config, IndentedWriter iw) {
            if (!Values.Any()) {
                iw.Write("[]");
                return;
            }

            bool singleLine = config.HasFlag(JsonStringifyOption.Inline);
            bool compact = config.HasFlag(JsonStringifyOption.Compact);

            if (singleLine) {
                iw.Write('[');
                if (!compact) iw.Write(' ');
            } else {
                iw.WriteLine('[');
                iw.IncreaseIndent();
            }

            bool first = true;
            foreach (var i in Values) {
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

                i.Stringify(config, iw);
            }

            if (singleLine) {
                if (!compact) iw.Write(' ');
                iw.Write(']');
            } else {
                iw.WriteLine();
                iw.DecreaseIndent();
                iw.Write(']');
            }
        }
    }
}
