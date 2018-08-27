using System.Linq;
using Core;
using NodaTime;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

namespace TestCore
{
    public class TestTrumfVisa
    {
        [Fact]
        public void TestNoTransaction()
        {
            var content = new[] {"lorem ipsum 232"};

            var parser = new TrumfVisa(content);
            var result = parser.GetTransactions();

            Assert.Empty(result);
        }

        [Fact]
        public void TestReadBalances()
        {
            var content = new[]
            {
                "Skyldig bel°p pr. 15.01.18 12.786,43"
            };

            var parser = new TrumfVisa(content);

            var result = parser.GetAccoutBalances();
            Assert.Single(result);
            Assert.Equal(-12786.43m, result.First().Amount);
            Assert.Equal(new LocalDate(2018, 1, 15), result.First().Date);
        }


        [Fact]
        public void TestReadEmptyBalance()
        {
            var content = new string[0];

            var parser = new TrumfVisa(content);

            Assert.Empty(parser.GetAccoutBalances());
        }

        [Fact]
        public void TestSingleTransaction()
        {
            var content = new[]
            {
                "09.01.18 St1 46137 Jerikoveie Oslo 08.01 30.01.18 511,26",
                "dummy line "
            };

            var parser = new TrumfVisa(content);
            var result = parser.GetTransactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Single(result);

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(new LocalDate(2018, 1, 9), resultTransaction.RecordDate);
            Assert.Equal(new LocalDate(2018, 1, 8), resultTransaction.TransactionDate);
            Assert.Equal(-511.26m, resultTransaction.Amount);
            Assert.Equal("St1 46137 Jerikoveie Oslo", resultTransaction.Description);
        }

        [Fact]
        public void TestSingleTransactionWithSpecilCases()
        {
            var content = new[] {"02.01.18 Clas Ohl 2852 Oslo 30.12 30.01.18 1 437,44"};

            var parser = new TrumfVisa(content);
            var result = parser.GetTransactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Single(result);

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(new LocalDate(2018, 1, 2), resultTransaction.RecordDate);
            Assert.Equal(new LocalDate(2017, 12, 30), resultTransaction.TransactionDate);
            Assert.Equal(-1437.44m, resultTransaction.Amount);
            Assert.Equal("NOK", resultTransaction.Currency);
        }

        [Fact]
        public void TestStrangeFormatting()
        {
            var content = new[]
            {
                "         03.04.18      Peixes Nice 31.03                                          30.04.18                   741,49",
                "Eur 75.3 Kurs 9.88653",
                "03.04.18      Bank Ocr                                                   30.03.18                                     22 329,31"
            };

            var parser = new TrumfVisa(content);
            var result = parser.GetTransactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(2, result.Count());

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal("Bank Ocr", result.ToArray()[1].Description);

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal("Eur", result.ToArray()[0].Currency);

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(-75.3m, result.ToArray()[0].CurAmount);
        }

        [Fact]
        public void TestStrangeFormatting2()
        {
            var content = new[]
            {
                "28.12.17 Bank Ocr 28.12.17 12 518,62"
            };

            var parser = new TrumfVisa(content);

            var result = parser.GetTransactions().FirstOrDefault();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.NotNull(result);

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal("Bank Ocr", result.Description);


            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(12518.62m, result.Amount);
        }
    }
}