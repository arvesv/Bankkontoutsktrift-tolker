using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace Core
{
    public class AccountState
    {
        public AccountState()
        {
            Amount = 0m;
            Date = new LocalDate();
        }

        public decimal Amount { get; }
        public LocalDate Date { get;  }
    }
}
