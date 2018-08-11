using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public abstract class ParserBase : IParser
    {
        internal readonly IEnumerable<string> _content;

        protected ParserBase(IEnumerable<string> content)
        {
            _content = content;
        }

        public abstract bool IsParseable { get; }

        public abstract IEnumerable<Trasaction> GetTransactions();

        protected bool Contains(string text)
        {
            return (_content.FirstOrDefault(line => line.Contains(text)) != null);
        }
    }
}
