using System.Collections.Generic;

namespace Core
{
    public interface IParser
    {
        bool IsParseable { get; }

        IEnumerable<Trasaction> GetTransactions();
    }
}