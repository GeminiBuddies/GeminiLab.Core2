using System;
using System.IO;

namespace GeminiLab.Core2.Markup.Json {
    public enum JsonTokenType {
        NotAToken,
        LBrace,         // {
        RBrace,         // }
        LBracket,       // [
        RBracket,       // ]
        Comma,          // ,
        Colon,          // :
        LiteralTrue,    // true
        LiteralFalse,   // false
        LiteralNull,    // null
        String,         // string
        Number,         // number
    }

    public ref struct JsonToken {
        public JsonTokenType Type;
        public ReadOnlySpan<char> Value;
        public int Row;
        public int Column;

        /*
        public JsonToken(JsonTokenType type, ReadOnlySpan<char> value, int row, int column) {
            Type = type;
            Value = value;
            Row = row;
            Column = column;
        }
        */

        public override string ToString() {
            return nameof(JsonToken) + $" {{ {Type.ToString()} \"{Value.ToString()}\" at ({Row}, {Column}) }}";
        }
    }

    public enum JsonGetTokenError {
        NoError,
        EndOfInput,
        UnknownLiteral,
        UnfinishedString,
    }

    public class JsonTokenizer {
        public bool IsLiteralCaseInsensitive { get; set; } = true;

        public JsonTokenizer(TextReader source) {
            _source = source;

            _r = 0; _ptr = 0; _len = 0;
            _line = string.Empty;
        }

        private readonly TextReader _source;
        private int _r;
        private string _line;
        private int _len, _ptr;

        private bool readLine() {
            if ((_line = _source.ReadLine()) == null) return false;

            ++_r; _ptr = 0; _len = _line.Length;
            return true;
        }

        private bool findNonspaceInLine() {
            while (_ptr < _len && char.IsWhiteSpace(_line, _ptr)) {
                ++_ptr;
            }

            return _ptr < _len;
        }

        private bool isSpecialChar(char c) => c == '[' || c == ']' || c == '{' || c == '}' || c == ':' || c == ',';

        private bool isLiteralChar(char c) {
            return !char.IsWhiteSpace(c) && !isSpecialChar(c) && c != '\"';
        }

        public JsonGetTokenError GetToken(out JsonToken token) {
            if (_line == null) { token = default; return JsonGetTokenError.EndOfInput; }

            for(;;) {
                if (findNonspaceInLine()) break;
                if (!readLine()) { token = default; return JsonGetTokenError.EndOfInput; }
            }

            char c = _line[_ptr];
            int begin = _ptr;
            token.Row = _r;
            token.Column = begin + 1;
            if (isSpecialChar(c)) {
                token.Type = c switch {
                    '[' => JsonTokenType.LBracket,
                    ']' => JsonTokenType.RBracket,
                    '{' => JsonTokenType.LBrace,
                    '}' => JsonTokenType.RBrace,
                    ':' => JsonTokenType.Colon,
                    ',' => JsonTokenType.Comma,
                    _ => JsonTokenType.NotAToken,
                };

                token.Value = _line.AsSpan(_ptr, 1);
                ++_ptr;
                return JsonGetTokenError.NoError;
            } else if (c == '\"') {
                ++_ptr;
                while (_ptr < _len) {
                    if (_line[_ptr] == '\"' && _line[_ptr - 1] != '\\') {
                        token.Type = JsonTokenType.String;
                        token.Value = _line.AsSpan(begin + 1, _ptr - begin - 1);
                        ++_ptr;
                        return JsonGetTokenError.NoError;
                    }

                    ++_ptr;
                }

                token.Type = JsonTokenType.String;
                token.Value = _line.AsSpan(begin + 1, _len - begin - 1);
                return JsonGetTokenError.UnfinishedString;
            } else {
                while (_ptr < _len && isLiteralChar(_line[_ptr])) ++_ptr;

                token.Value = _line.AsSpan(begin, _ptr - begin);

                if (token.Value.Equals("true", IsLiteralCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
                    token.Type = JsonTokenType.LiteralTrue; return JsonGetTokenError.NoError;
                }

                if (token.Value.Equals("false", IsLiteralCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
                    token.Type = JsonTokenType.LiteralFalse; return JsonGetTokenError.NoError;
                }

                if (token.Value.Equals("null", IsLiteralCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
                    token.Type = JsonTokenType.LiteralNull; return JsonGetTokenError.NoError;
                }

                if (int.TryParse(token.Value, out _) || double.TryParse(token.Value, out _)) {
                    token.Type = JsonTokenType.Number;
                    return JsonGetTokenError.NoError;
                }

                token.Type = JsonTokenType.NotAToken;
                return JsonGetTokenError.UnknownLiteral;
            }
        }
    }
}