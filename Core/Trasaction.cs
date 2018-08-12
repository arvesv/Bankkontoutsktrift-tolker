using NodaTime;

namespace Core
{
    public class Trasaction
    {
        public decimal Amount;
        public decimal CurAmount;
        public string Currency;
        public string Description;
        public LocalDate RecordDate;
        public LocalDate TransactionDate;
        public string Bank;
        public string Accout;

        public override string ToString()
        {
            return string.Format("{0:d} {1,-40} {2,15}", TransactionDate, Description, Amount);
        }
    }
}