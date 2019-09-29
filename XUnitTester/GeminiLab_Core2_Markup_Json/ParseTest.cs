using GeminiLab.Core2.Markup.Json;
using Xunit;

namespace XUnitTester.GeminiLab_Core2_Markup_Json {
    public static class ParseTest {
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

            Assert.Equal("{ \"a\": null, \"b\": [ 23.4444, 433336 ], \"ca\": { \"d\": [ true, false, \"ame\", \"冬好き\" ], \"e\": [], \"f\": {} } }", result.ToString());
            Assert.Equal("{ \"a\": null, \"b\": [ 23.4444, 433336 ], \"ca\": { \"d\": [ true, false, \"ame\", \"\\u51ac\\u597d\\u304d\" ], \"e\": [], \"f\": {} } }", result.ToString(JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly));
            Assert.Equal("{\"a\":null,\"b\":[23.4444,433336],\"ca\":{\"d\":[true,false,\"ame\",\"\\u51ac\\u597d\\u304d\"],\"e\":[],\"f\":{}}}", result.ToString(JsonStringifyOption.Inline | JsonStringifyOption.AsciiOnly | JsonStringifyOption.Compact));
            Assert.Equal("{\n    \"a\": null,\n    \"b\": [\n        23.4444,\n        433336\n    ],\n    \"ca\": {\n        \"d\": [\n            true,\n            false,\n            \"ame\",\n            \"冬好き\"\n        ],\n        \"e\": [],\n        \"f\": {}\n    }\n}", result.ToString(JsonStringifyOption.None, "\n"));
        }
    }
}
