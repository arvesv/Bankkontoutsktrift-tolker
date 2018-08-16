using System.Linq;
using Core;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

namespace TestCore
{
    public class TestNorwegianKortet
    {
        [Fact]
        public void TestMiscTransaction()
        {
            var content = new[]
            {
                "21.03.2018 SALATERIET AS, OSLO 469279 * *****7959 59,00 NOK 59,00",
                "14.06.2018 EASYLUNSJ,TONSBERG 469279******7959 14,00 NOK 14,00",
                "25.03.2018 CRAVATTIFICIO ZADI 654,MILANO 469279******7959 30,00 EUR 9.7717 293,15",
                "15.06.2018 Innbetaling fra 97101284771 - -12.944,40",
                "Norwegian-kortet"
            };

            var result = Utilities.Parse(content);
            Assert.Equal(4, result.Count());

            var results = result.ToArray();
            Assert.Equal(-59m - 14m - 293.15m + 12944.40m, results.Sum(t => t.Amount));


            Assert.Equal("CRAVATTIFICIO ZADI 654,MILANO", results[2].Description);
            Assert.Equal("EUR", results[2].Currency);
            Assert.Equal(-30m, results[2].CurAmount);
        }

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
            Assert.Equal("NOK", resultTransaction.Currency);
            Assert.Equal(-59.00m, resultTransaction.CurAmount);
        }
    }
}