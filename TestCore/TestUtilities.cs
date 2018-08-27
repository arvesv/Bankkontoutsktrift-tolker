using Core;
using NodaTime;
using Xunit;

namespace TestCore
{
    public class TestUtilities
    {
        [Fact]
        public void TestDateDiff()
        {
            Assert.Equal(0, Utilities.DateDiff(new LocalDate(2018, 1, 2), new LocalDate(2018, 1, 2)));

            Assert.Equal(1, Utilities.DateDiff(new LocalDate(2018, 1, 3), new LocalDate(2018, 1, 2)));

            Assert.Equal(1, Utilities.DateDiff(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 2)));

            Assert.Equal(31, Utilities.DateDiff(new LocalDate(2018, 1, 1), new LocalDate(2018, 2, 1)));

            Assert.Equal(1, Utilities.DateDiff(new LocalDate(2018, 1, 1), new LocalDate(2017, 12, 31)));
        }
    }
}