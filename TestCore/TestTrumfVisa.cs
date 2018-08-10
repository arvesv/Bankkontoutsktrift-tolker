using System.Linq;
using NodaTime;
using Xunit;

namespace TestCore
{
    public class TestTrumfVisa
    {

        [Fact]
        public void TestNoTransaction()
        {
            var content = new[] { "lorem ipsum 232" };

            var parser = new Core.TrumfVisa(content);
            var result = parser.GeTrasactions();

            Assert.Empty(result);
        }

        [Fact]
        public void TestSingleTransaction()
        {
            var content = new[] {"09.01.18 St1 46137 Jerikoveie Oslo 08.01 30.01.18 511,26"};

            var parser = new Core.TrumfVisa(content);
            var result = parser.GeTrasactions();

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Single(result);

            // ReSharper disable once PossibleMultipleEnumeration
            var resultTransaction = result.First();

            Assert.Equal(new LocalDate(2018,1,9), resultTransaction.RecordDate);
            Assert.Equal(new LocalDate(2018, 1, 8), resultTransaction.TransactionDate);
            Assert.Equal(511.26m, resultTransaction.Amount);

        }
    }
}
