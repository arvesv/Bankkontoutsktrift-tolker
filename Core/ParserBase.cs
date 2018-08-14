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

        public virtual (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            return (null, false);
        }

        public virtual IEnumerable<Transaction> GetTransactions()
        {
            using (var enumerator = Content.GetEnumerator())
            {
                var hasReadAhead = false;

                while (hasReadAhead || enumerator.MoveNext())
                {
                    Transaction trans = null;

                    (trans, hasReadAhead) = ParseLine(enumerator);


                    if (trans != null)
                    {
                        yield return trans;
                        continue;
                    }
                }
            }
        }

        public string Source { get; set; }

        public string Name { get; set; }
        public string Account { get; set; }

        protected bool Contains(string text)
        {
            return Content.FirstOrDefault(line => line.Contains(text)) != null;
        }
    }
}