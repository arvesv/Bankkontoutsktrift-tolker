using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void TestSingleTransaction()
        {
            var content = new[]
            {
                "21.03.2018 SALATERIET AS, OSLO 469279 * *****7959 59,00 NOK 59,00",
                "Norwegian-kortet"
            };

            var result = Utilities.Parse(content);
            Assert.Single(result);

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(-59.00m, resultTransaction.Amount);
            Assert.Equal("SALATERIET AS, OSLO", resultTransaction.Description);
        }
    }
}