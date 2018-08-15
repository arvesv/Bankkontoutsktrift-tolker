using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
// ReSharper disable PossibleMultipleEnumeration

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

            var resultTransaction = result.First();

            Assert.Equal(-59.00m, resultTransaction.Amount);
            Assert.Equal("SALATERIET AS, OSLO", resultTransaction.Description);
        }

        [Fact]
        public void TestMiscTransaction()
        {
            var content = new[]
            {
                "21.03.2018 SALATERIET AS, OSLO 469279 * *****7959 59,00 NOK 59,00",
                "14.06.2018 EASYLUNSJ,TONSBERG 469279******7959 14,00 NOK 14,00",
                "25.03.2018 CRAVATTIFICIO ZADI 654,MILANO 469279******7959 30,00 EUR 9.7717 293,15",
                "Norwegian-kortet"
            };

            var result = Utilities.Parse(content);
            Assert.Equal(3, result.Count());

            Transaction[] results = result.ToArray();
            Assert.Equal(59m + 14m + 293.15m, -results.Sum(t => t.Amount));


            Assert.Equal("CRAVATTIFICIO ZADI 654,MILANO", results[2].Description);


        }
    }
}
