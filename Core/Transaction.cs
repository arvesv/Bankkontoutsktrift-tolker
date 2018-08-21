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
            return $"{TransactionDate:d} {Description,-40} {Amount,15} {Currency,4} {CurAmount,15}";
        }
    }
}