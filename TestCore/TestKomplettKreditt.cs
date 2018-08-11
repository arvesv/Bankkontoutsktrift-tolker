using System.Linq;
using Core;
using NodaTime;
using Xunit;

namespace TestCore
{
    public class TestKomplettKreditt
    {
        [Fact]
        public void TestCheckIsKomplett()
        {
            var content = new[] { "Komplett Bank MasterCard" };

            var parser = new KomplettKreditt(content);
            Assert.True(parser.IsParseable);
        }

        [Fact]
        public void TestSingleLoaclCurrencyTransaction()
        {
            var content = new[]
            {
                "14.03.2018           Bit OSL Gardermoen, Edvard Munchs veg Gardermoen                                                          1 234,56"
            };

            var parser = new KomplettKreditt(content);
            var result = parser.GeTrasactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Single(result);

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(new LocalDate(2018, 3, 14), resultTransaction.TransactionDate);
            Assert.Equal(-1234.56m, resultTransaction.Amount);
            Assert.Equal("Bit OSL Gardermoen, Edvard Munchs veg Gardermoen", resultTransaction.Description);
        }

    }
}
