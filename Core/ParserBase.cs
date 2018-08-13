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

        public abstract IEnumerable<Transaction> GetTransactions();

        public string Source { get; set; }

        public string Name { get; set; }
        public string Account { get; set; }

        protected bool Contains(string text)
        {
            return Content.FirstOrDefault(line => line.Contains(text)) != null;
        }
    }
}