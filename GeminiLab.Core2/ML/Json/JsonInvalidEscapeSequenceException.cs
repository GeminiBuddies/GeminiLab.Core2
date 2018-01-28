namespace GeminiLab.Core2.ML.Json {
    public class JsonInvalidEscapeSequenceException : JsonParsingException {
        public JsonInvalidEscapeSequenceException(string sequence) {
            Sequence = sequence;
        }

        public string Sequence { get; }
    }
}