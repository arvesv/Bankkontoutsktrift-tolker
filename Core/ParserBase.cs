﻿using System.Collections.Generic;
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

        public string DefaultCurrency => "NOK";

        public virtual bool IsParseable => false;

        public virtual IEnumerable<Transaction> GetTransactions()
        {
            using (var enumerator = Content.GetEnumerator())
            {
                var hasReadAhead = false;

                while (hasReadAhead || enumerator.MoveNext())
                {
                    Transaction trans;

                    (trans, hasReadAhead) = ParseLine(enumerator);


                    if (trans != null) yield return trans;
                }
            }
        }

        public string Source { get; set; }

        public string Name { get; set; }
        public string Account { get; set; }

        public virtual IEnumerable<AccountBalance> GetAccoutBalances()
        {
            foreach (var line in Content)
            {
                var stmt = ParserStatmentLine(line);
                if (stmt != null) yield return stmt;
            }
        }

        public virtual (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            return (null, false);
        }

        protected bool Contains(string text)
        {
            return Content.FirstOrDefault(line => line.Contains(text)) != null;
        }

        public virtual AccountBalance ParserStatmentLine(string line)
        {
            return null;
        }
    }
}