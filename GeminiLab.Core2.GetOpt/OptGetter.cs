using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace GeminiLab.Core2.GetOpt {
    public enum OptionType {
        Switch,
        Parameterized,
        MultiParameterized,
    }

    public enum GetOptError {
        NoError,
        UnknownOption,
        UnexpectedValue,
        UnexpectedAttachedValue,
        ExpectedValue,
        EndOfArguments,
    }

    public enum GetOptResultType {
        ShortOption,
        LongOption,
        LongAlias,
        Values,
        Invalid,
    }

    public struct GetOptResult {
        public GetOptResultType Type;
        public char Option;                 // set to '\0' if Type is LongOption or Values
        public string LongOption;           // set to null if Type is ShortOption or Values
        public string Parameter;
        public string[] Parameters;
    }

    public class OptGetter {
        private readonly Dictionary<char, OptionType> _options = new Dictionary<char, OptionType>();
        private readonly Dictionary<string, OptionType> _longOptions = new Dictionary<string, OptionType>();
        private readonly Dictionary<string, char> _aliases = new Dictionary<string, char>();

        public void AddOption(char option, OptionType type) {
            _options[option] = type;
        }

        public void AddOption(char option, OptionType type, params string[] longAliases) {
            AddOption(option, type);
            AddAlias(option, longAliases);
        }

        public void AddOption(string longOption, OptionType type) {
            _longOptions.Add(longOption, type);
        }

        public void AddAlias(char option, params string[] longAliases) {
            foreach (var alias in longAliases) {
                _aliases.Add(alias, option);
            }
        }

        public bool EnableDashDash { get; set; } = true;

        private string[] _args = null;
        private int _argc;
        private int _argp;
        public void BeginParse(params string[] arguments) {
            _args = arguments;
            _argc = _args.Length;
            _argp = 0;
        }

        private bool isOption(string v, out char c) {
            c = '\0';
            if (v.Length > 1 && v[0] == '-') {
                if (v[1] == '-') return v.Length > 2 || !EnableDashDash;
                
                c = v[1];
                return true;
            }

            return false;
        }

        private bool isEnabledDashDash(string v) {
            return EnableDashDash && v == "--";
        }

        private bool tryGetValue(out string v) {
            if (_argp >= _argc) {
                v = null;
                return false;
            }

            v = _args[_argp];
            if (isOption(v, out _) || isEnabledDashDash(v)) return false;

            ++_argp;
            return true;
        }

        public GetOptError GetOpt(out GetOptResult result) {
            result = new GetOptResult {
                Type = GetOptResultType.Invalid,
                Option = '\0',
                LongOption = null,
                Parameter = null,
                Parameters = null
            };

            if (_args == null || _argp >= _argc) return GetOptError.EndOfArguments;

            string v = _args[_argp++];
            
            if (isOption(v, out char c)) {
                OptionType type;

                if (c != '\0' && v.Length > 2) {
                    result.Parameter = v.Substring(2); // attached value
                }

                if (c != '\0') {
                    result.Type = GetOptResultType.ShortOption;
                    result.Option = c;
                    if (!_options.TryGetValue(c, out type)) {
                        return GetOptError.UnknownOption;
                    }
                } else {
                    string option = v.Substring(2);
                    result.LongOption = option;
                    result.Type = GetOptResultType.LongOption;

                    if (!_longOptions.TryGetValue(option, out type)) {
                        if (_aliases.TryGetValue(option, out result.Option)) {
                            result.Type = GetOptResultType.LongAlias;
                            if (!_options.TryGetValue(result.Option, out type)) return GetOptError.UnknownOption;
                        } else {
                            return GetOptError.UnknownOption;
                        }
                    }
                }

                if (type == OptionType.Switch) {
                    return result.Parameter != null ? GetOptError.UnexpectedAttachedValue : GetOptError.NoError;
                }

                if (type == OptionType.Parameterized) {
                    if (result.Parameter != null || tryGetValue(out result.Parameter)) {
                        return GetOptError.NoError;
                    }

                    return GetOptError.ExpectedValue;
                }

                List<string> p = new List<string>();
                if (result.Parameter != null) {
                    p.Add(result.Parameter);
                    result.Parameter = null;
                }

                while (tryGetValue(out var s)) p.Add(s);

                result.Parameters = p.ToArray();
                return GetOptError.NoError;
            } else if (isEnabledDashDash(v)) {
                int len = _argc - _argp;
                result.Parameters = new string[len];
                result.Type = GetOptResultType.Values;

                Array.Copy(_args, _argp, result.Parameters, 0, len);
                _argp = _argc;
                return GetOptError.NoError;
            } else {
                result.Parameter = v;
                return GetOptError.UnexpectedValue;
            }
        }

        public void EndParse() {
            _args = null;
        }
    }
}
