using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeminiLab.Core2.GetOpt;

namespace GeminiLab.Core2.CmdParser {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class OptionAttribute : Attribute {
        public OptionAttribute() { }

        public char Option { get; set; } = '\0';
        public string LongOption { get; set; } = null;
    }

    public static class CommandLineParser<T> {
        // ReSharper disable StaticMemberInGenericType
        private static readonly object InternalLock = new object();
        private static OptGetter _opt = null;
        private static Dictionary<char, PropertyInfo> _shortOptionTargets = null;
        private static Dictionary<string, PropertyInfo> _longOptionTargets = null;
        // ReSharper restore StaticMemberInGenericType

        private static OptGetter generateOptGetter() {
            var opt = new OptGetter();
            _shortOptionTargets = new Dictionary<char, PropertyInfo>();
            _longOptionTargets = new Dictionary<string, PropertyInfo>();

            var type = typeof(T);

            var props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props) {
                var propType = prop.PropertyType;
                var optionType = OptionType.Switch;

                if (propType == typeof(bool)) {
                    optionType = OptionType.Switch;
                } else if (propType == typeof(string)) {
                    optionType = OptionType.Parameterized;
                } else if (propType == typeof(string[])) {
                    optionType = OptionType.MultiParameterized;
                } else {
                    continue;
                }

                foreach (var attr in prop.GetCustomAttributes(typeof(OptionAttribute), true).OfType<OptionAttribute>()) {
                    if (attr.Option != '\0') {
                        _shortOptionTargets[attr.Option] = prop;
                        opt.AddOption(attr.Option, optionType);
                    }

                    if (attr.LongOption != null) {
                        _longOptionTargets[attr.LongOption] = prop;
                        opt.AddOption(attr.LongOption, optionType);
                    }
                }
            }

            return opt;
        }

        public static T Parse(params string[] args) {
            lock (InternalLock) {
                if (_opt == null) _opt = generateOptGetter();

                _opt.BeginParse(args);

                T rv = Activator.CreateInstance<T>();

                GetOptError err;
                while ((err = _opt.GetOpt(out var result)) != GetOptError.EndOfArguments) {
                    if (err == GetOptError.NoError) {
                        PropertyInfo prop;

                        switch (result.Type) {
                        case GetOptResultType.ShortOption:
                        case GetOptResultType.LongAlias:    // not supposed to happen, but handle it anyway
                            prop = _shortOptionTargets[result.Option];
                            break;
                        case GetOptResultType.LongOption:
                            prop = _longOptionTargets[result.LongOption];
                            break;
                        // case GetOptResultType.Values:
                        // case GetOptResultType.Invalid:
                        default:
                            continue;
                        }

                        switch (result.OptionType) {
                        case OptionType.Switch:
                            prop.SetValue(rv, true);
                            break;
                        case OptionType.Parameterized:
                            prop.SetValue(rv, result.Argument);
                            break;
                        case OptionType.MultiParameterized:
                            prop.SetValue(rv, result.Arguments);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                _opt.EndParse();
                return rv;
            }
        }
    }
}
