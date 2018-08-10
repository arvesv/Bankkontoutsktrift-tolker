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
    }
}