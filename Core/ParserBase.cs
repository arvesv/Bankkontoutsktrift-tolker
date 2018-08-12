using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public abstract class ParserBase : IParser
    {
        internal readonly IEnumerable<string> Content;

        protected ParserBase(IEnumerable<string> content)
        {
            Content = content;
        }

        public abstract bool IsParseable { get; }

        public abstract IEnumerable<Trasaction> GetTransactions();

        protected bool Contains(string text)
        {
            return Content.FirstOrDefault(line => line.Contains(text)) != null;
        }
    }
}