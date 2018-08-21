using System.Collections.Generic;

namespace Core
{
    public interface IParser
    {
        bool IsParseable { get; }
        string Source { get; set; }
        string Name { get; set; }
        string Account { get; set; }

        IEnumerable<Transaction> GetTransactions();

        IEnumerable<AccountState> GetAccoutStates();
    }
}