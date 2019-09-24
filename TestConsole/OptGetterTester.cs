using GeminiLab.Core2;
using GeminiLab.Core2.GetOpt;

namespace TestConsole {
    class OptGetterTester {
        private static string mix(char c, string s) {
            if (c == '\0') return s ?? "<null>";
            return s == null ? new string(c, 1) : $"{c}|{s}";
        }

        public static void TestOptGetter(OptGetter opt, params string[] p) {
            // GetOptResult r = default;
            Exconsole.WriteLine(">" + p.JoinBy(" "));
            opt.BeginParse(p);

            bool eoa = false;
            GetOptError err;
            while (!eoa) {
                if ((err = opt.GetOpt(out var result)) == GetOptError.EndOfArguments) {
                    eoa = true;
                }

                Exconsole.WriteLine($"  {err}: {result.Type}: \"{mix(result.Option, result.LongOption)}\", p: {result.Argument ?? "<null>"}, pp: {result.Arguments?.JoinBy(", ") ?? "<null>"}");
            }

            opt.EndParse();
        }
    }
}
