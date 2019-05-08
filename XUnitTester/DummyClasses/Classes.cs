using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTester.DummyClasses {
    internal class Base {
    }

    internal class A : Base {
        public int Num { get; set; }
    }

    internal class B : Base {
        public string Str { get; set; }
    }
}
