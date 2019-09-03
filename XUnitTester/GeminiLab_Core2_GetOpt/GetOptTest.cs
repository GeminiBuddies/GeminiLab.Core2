using System;
using System.Collections.Generic;
using System.Text;
using GeminiLab.Core2.GetOpt;
using Xunit;

namespace XUnitTester.GeminiLab_Core2_GetOpt {
    public class GetOptTest {
        private static OptGetter optGetter() {
            var rv = new OptGetter();

            rv.AddOption('a', OptionType.Switch, "alpha");
            rv.AddOption('b', OptionType.Parameterized, "bravo");
            rv.AddOption('c', OptionType.MultiParameterized, "charlie");

            return rv;
        }

        [Fact]
        public static void IllOptionTest() {
            var opt = optGetter();
            opt.AddOption('i', (OptionType)7777, "invalid");

            opt.BeginParse("-i");

            Assert.Equal(GetOptError.UnknownOption, opt.GetOpt(out var result));
            Assert.Equal(GetOptResultType.ShortOption, result.Type);
            Assert.Equal('i', result.Option);
            Assert.Null(result.LongOption);
            Assert.Null(result.Argument);
            Assert.Null(result.Arguments);
        }
    }
}
