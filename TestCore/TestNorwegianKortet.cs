using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Xunit;

namespace TestCore
{
    public class TestNorwegianKortet
    {
        [Fact]
        public void TestNoTransaction()
        {
            var content = new[]
            {
                "lorem ipsum 232",
                "Norwegian-kortet"
            };

            var parser = new NorwegianKortet(content);
            Assert.True(parser.IsParseable);

            var result = parser.GetTransactions();
            Assert.Empty(result);
        }

    }
}
