using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class NorwegianKortet : ParserBase
    {
        public NorwegianKortet(IEnumerable<string> content) : base(content)
        {
        }

        public override bool IsParseable => Contains("Norwegian-kortet");
    }
}
