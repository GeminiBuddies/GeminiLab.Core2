using System;
using System.Collections.Generic;
using System.Text;

namespace GeminiLab.Core2.Logger.Layouts {
    public class LambdaLayout : ILayout {
        private readonly Func<int, string, string, string> _lambda;

        public LambdaLayout(Func<int, string, string, string> lambda) => _lambda = lambda;

        public string Format(int level, string category, string content) => _lambda(level, category, content);

        public static implicit operator LambdaLayout(Func<int, string, string, string> lambda) => new LambdaLayout(lambda);
    }
}
