using System.Collections.Generic;

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