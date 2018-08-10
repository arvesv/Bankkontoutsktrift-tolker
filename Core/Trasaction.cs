using NodaTime;
using System;

namespace Core
{
    public class Trasaction
    {
        public LocalDate TransactionDate;
        public LocalDate RecordDate;
        public decimal Amount;
        public string Description;
        public string Currency;
        public decimal CurAmount;
    }
}
