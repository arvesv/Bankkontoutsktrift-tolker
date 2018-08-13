using NodaTime;

namespace Core
{
    public class Transaction
    {
        public string Accout;
        public decimal Amount;
        public string Bank;
        public decimal CurAmount;
        public string Currency;
        public string Description;
        public LocalDate RecordDate;
        public LocalDate TransactionDate;

        public override string ToString()
        {
            return string.Format("{0:d} {1,-40} {2,15}", TransactionDate, Description, Amount);
        }
    }
}