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
            var content = new[] {"Komplett Bank MasterCard"};

            var parser = new KomplettKreditt(content);
            Assert.True(parser.IsParseable);
        }

        [Fact]
        public void TestCurrencyTransaction()
        {
            var content = new[]
            {
                "15.03.2018           Hunter Bar 5019055, Oslo Lufthavn Gardemoen                                                                262,00",
                "16.03.2018           CARREFOUR GRANA, CRTA DE ARMILLA S/N GRANADA",
                "25,52 EUR / Kurs 9,760190                                                                                  249,08"
            };

            var parser = new KomplettKreditt(content);
            var result = parser.GetTransactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(2, result.Count());

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.Skip(1).First();

            Assert.Equal(new LocalDate(2018, 3, 16), resultTransaction.TransactionDate);
            Assert.Equal(-249.08m, resultTransaction.Amount);
            Assert.Equal("CARREFOUR GRANA, CRTA DE ARMILLA S/N GRANADA", resultTransaction.Description);

            Assert.Equal("EUR", resultTransaction.Currency);
            Assert.Equal(-25.52m, resultTransaction.CurAmount);
        }

        [Fact]
        public void TestPaymentInn()
        {
            var content = new[]
            {
                "01.02.2018 Rema 1000 Torshov, Sandakerveien 24 Oslo -120,00",
                "20.02.2018 Innbetaling -2 938,50"
            };

            var parser = new KomplettKreditt(content);
            var result = parser.GetTransactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(2, result.Count());

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(new LocalDate(2018, 2, 1), resultTransaction.TransactionDate);
            Assert.Equal(120m, resultTransaction.Amount);
            Assert.Equal("Rema 1000 Torshov, Sandakerveien 24 Oslo", resultTransaction.Description);
        }

        [Fact]
        public void TestSingleLoaclCurrencyTransaction()
        {
            var content = new[]
            {
                "14.03.2018           Bit OSL Gardermoen, Edvard Munchs veg Gardermoen                                                          1 234,56"
            };

            var parser = new KomplettKreditt(content);
            var result = parser.GetTransactions();

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