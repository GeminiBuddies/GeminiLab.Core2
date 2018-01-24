namespace GeminiLab.Core2.ML.Json {
    public class JsonParsingUnknownSymbolException : JsonParsingException {
        public string Symbol { get; }
        public int Row { get; }
        public int Column { get; }

        internal JsonParsingUnknownSymbolException(JsonToken tok) : this(tok.Value, tok.Row, tok.Column) {
        }

        public JsonParsingUnknownSymbolException(string symbol, int row, int column) {
            Symbol = symbol;
            Row = row;
            Column = column;
        }
    }
}
