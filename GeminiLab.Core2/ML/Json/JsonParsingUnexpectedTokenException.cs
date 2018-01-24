namespace GeminiLab.Core2.ML.Json {
    public class JsonParsingUnexpectedTokenException : JsonParsingException {
        public string Token { get; }
        public int Row { get; }
        public int Column { get; }

        internal JsonParsingUnexpectedTokenException(JsonToken tok) : this(tok.Value, tok.Row, tok.Column) {
        }

        public JsonParsingUnexpectedTokenException(string token, int row, int column) {
            Token = token;
            Row = row;
            Column = column;
        }
    }
}
