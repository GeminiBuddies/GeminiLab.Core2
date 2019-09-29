using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using GeminiLab.Core2.Markup.Json;
using Xunit;

namespace XUnitTester.GeminiLab_Core2_Markup_Json {
    public static class Test {
        [Fact]
        public static void ParseAndStringifyTest() {
            var result = JsonParser.Parse("{ \"a\": \n\n\tnull, \"b\": [  23.4444, 433336 ]\r\r, \"ca\": { \"d\" : [ true, false, \"ame\", \"冬好き\" ], \"e\":[], \"f\":{} } }");

            if (!(result is JsonObject root)) { Assert.True(false); return; }
            Assert.Equal(3, root.Count);
            if (!(root.TryGetValue("a", out var rootA))) { Assert.True(false); return; }
            Assert.True(rootA is JsonNull);
            if (!(root.TryGetValue("b", out var rootB)) || !(rootB is JsonArray rootBArr)) { Assert.True(false); return; }
            Assert.Equal(2, rootBArr.Count);
            if (!(rootBArr[0] is JsonNumber rootB0) || !rootB0.IsFloat) { Assert.True(false); return; }
            Assert.Equal(23.4444, rootB0.ValueFloat);
            if (!(rootBArr[1] is JsonNumber rootB1) || rootB1.IsFloat) { Assert.True(false); return; }
            Assert.Equal(433336, rootB1.ValueInt);
            if (!(root.TryGetValue("ca", out var rootCa)) || !(rootCa is JsonObject rootCaObj)) { Assert.True(false); return; }
            Assert.Equal(3, rootCaObj.Count);
            if (!(rootCaObj.TryGetValue("d", out var rootCaD)) || !(rootCaD is JsonArray rootCaDArr)) { Assert.True(false); return; }
            Assert.Equal(4, rootCaDArr.Count);
            Assert.True(rootCaDArr[0] is JsonBool rootCaD0 && rootCaD0.Value);
            Assert.True(rootCaDArr[1] is JsonBool rootCaD1 && !rootCaD1.Value);
            Assert.True(rootCaDArr[2] is JsonString rootCaD2 && rootCaD2 == "ame");
            Assert.True(rootCaDArr[3] is JsonString rootCaD3 && rootCaD3 == "冬好き");
            if (!(rootCaObj.TryGetValue("e", out var rootCaE)) || !(rootCaE is JsonArray rootCaEArr)) { Assert.True(false); return; }
            Assert.Empty(rootCaEArr);
            if (!(rootCaObj.TryGetValue("f", out var rootCaF)) || !(rootCaF is JsonObject rootCaFObj)) { Assert.True(false); return; }
            Assert.Equal(0, rootCaFObj.Count);

            Assert.Equal("{ \"a\": null, \"b\": [ 23.4444, 433336 ], \"ca\": { \"d\": [ true, false, \"ame\", \"冬好き\" ], \"e\": [], \"f\": {} } }", result.ToString(JsonStringifyOption.Inline));
            Assert.Equal("{ \"a\": null, \"b\": [ 23.4444, 433336 ], \"ca\": { \"d\": [ true, false, \"ame\", \"\\u51ac\\u597d\\u304d\" ], \"e\": [], \"f\": {} } }", result.ToString(JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly));
            Assert.Equal("{\"a\":null,\"b\":[23.4444,433336],\"ca\":{\"d\":[true,false,\"ame\",\"\\u51ac\\u597d\\u304d\"],\"e\":[],\"f\":{}}}", result.ToString(JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly | JsonStringifyOption.Compact));
            Assert.Equal("{\n    \"a\": null,\n    \"b\": [\n        23.4444,\n        433336\n    ],\n    \"ca\": {\n        \"d\": [\n            true,\n            false,\n            \"ame\",\n            \"冬好き\"\n        ],\n        \"e\": [],\n        \"f\": {}\n    }\n}", result.ToString(JsonStringifyOption.None, "\n"));
        }

        [Fact]
        public static void JsonBoolMethodsTest() {
            JsonBool boolA = new JsonBool(false), boolB = false, boolC = true;
            object objTrue = true, objFalse = false;

            Assert.True(boolA == boolB);
            Assert.False(boolA == boolC);
            Assert.False(boolA != boolB);
            Assert.True(boolA != boolC);
            Assert.True((JsonBool)null == null);

            Assert.True(boolA.Equals((object)boolA));
            Assert.True(boolA.Equals((object)boolB));
            Assert.False(boolC.Equals((object)boolA));
            Assert.True(boolA.Equals(boolA));
            Assert.True(boolA.Equals(boolB));
            Assert.False(boolC.Equals(boolA));
            Assert.True(boolC.Equals(objTrue));
            Assert.False(boolC.Equals(objFalse));
            Assert.False(boolC.Equals((object)null));
            Assert.False(boolC.Equals((JsonBool)null));

            Assert.True(boolC);
            Assert.False(boolB);

            Assert.Equal(true.GetHashCode(), boolC.GetHashCode());
            Assert.Equal(false.GetHashCode(), boolA.GetHashCode());
        }

        [Fact]
        public static void JsonNullMethodsTest() {
            JsonNull nullA = new JsonNull(), nullB = new JsonNull();
            
            Assert.True(nullA.Equals(nullB));
            Assert.True(nullA.Equals((object)null));
            Assert.True(nullA.Equals((object)nullB));

            Assert.True(nullA == nullB);
            Assert.False(nullA != nullB);

            Assert.Equal(0, nullA.GetHashCode());
        }

        [Fact]
        public static void JsonNumberMethodsTest() {
            JsonNumber intA = 233, intB = 666, intC = 666, floatA = 43.25, floatB = 96.625, floatC = 96.625;
            object objA = "";

            Assert.True(floatA.IsFloat);
            Assert.True(floatB.IsFloat);
            Assert.False(intA.IsFloat);
            Assert.False(intB.IsFloat);

            Assert.True(intB == intC);
            Assert.False(intB != intC);

            Assert.Equal<double>(233, intA);
            Assert.Equal<double>(43.25, floatA);

            Assert.True(intB.Equals(intC));
            Assert.False(intA.Equals(intB));
            Assert.False(intA.Equals(floatA));
            Assert.True(floatB.Equals(floatC));

            Assert.True(intB.Equals((object)intB));
            Assert.True(intB.Equals((object)intC));
            Assert.False(intB.Equals(objA));
            Assert.False(intB.Equals((object)null));
            Assert.False(intB.Equals(null));

            Assert.Equal(intB.ValueInt.GetHashCode(), intB.GetHashCode());
            Assert.Equal(floatB.ValueFloat.GetHashCode(), floatB.GetHashCode());
            Assert.Throws<ArgumentOutOfRangeException>(() => new JsonNumber("invalid"));
        }

        [Fact]
        public static void JsonArrayMethodsTest() {
            var arr = new JsonArray();
            var list = new List<JsonValue>();
            JsonValue value;

            Assert.False(arr.IsReadOnly);

            value = (JsonString)"aloha oe";
            arr.Add(value);
            list.Add(value);

            value = (JsonNumber)4242;
            arr.Add(value);
            list.Add(value);

            value = (JsonBool)false;
            arr.Add(value);
            list.Add(value);

            value = new JsonNull();
            arr.Add(value);
            list.Add(value);

            Assert.Equal(4, arr.Count);

            var varr = new JsonValue[4];
            arr.CopyTo(varr, 0);

            Assert.Equal(list, varr);

            int ptr = 0;
            foreach (var v in arr) {
                Assert.Equal(list[ptr++], v);
            }

            value = (JsonBool)true;
            arr[2] = value;
            Assert.Equal(value, arr[2]);
            Assert.Equal(2, arr.IndexOf(value));
            Assert.Contains(value, arr);
            Assert.True(arr.Remove(value));
            arr.RemoveAt(2);
            Assert.Equal(2, arr.Count);
            arr.Insert(2, value);
            Assert.Equal(value, arr[2]);
            arr.Clear();
            Assert.Empty(arr);
        }
    }
}
