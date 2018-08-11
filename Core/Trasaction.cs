using System;
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

        public override string ToString()
        {
            return String.Format("{0:d} {1,-40} {2,15}", TransactionDate, Description, Amount);
        }
    }
}