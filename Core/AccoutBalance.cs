using NodaTime;

namespace Core
{
    public class AccountBalance
    {
        public AccountBalance()
        {
            Amount = 0m;
            Date = new LocalDate();
        }

        public decimal Amount { internal set; get; }
        public LocalDate Date { internal set; get; }
    }
}