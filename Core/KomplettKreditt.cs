using System.Collections.Generic;
using System.Text.RegularExpressions;
using NodaTime.Text;

namespace Core
{
    public class KomplettKreditt : ParserBase
    {
        private static readonly LocalDatePattern
            DatePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yyyy");

        // A text that only apprears in an Komplett Kredittkort invoice
        private readonly string MagicText = "Komplett Bank MasterCard";

        public KomplettKreditt(string[] content)
            : base(content)
        {
            Name = MagicText;
        }

        public override bool IsParseable => Contains(MagicText);


        public override IEnumerable<Trasaction> GetTransactions()
        {
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = Content.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var line = enumerator.Current;
                var transaction = ReadTransaction(line);
                if (transaction != null)
                {
                    yield return transaction;
                }
                else
                {
                    transaction = ReadCurrencyTransaction(enumerator);
                    if (transaction != null) yield return transaction;
                }
            }
        }

        private Trasaction ReadTransaction(string line)
        {
            var match = Regex.Match(line, @"^(\d{2}\.\d{2}\.\d{4})\s+(.*\D)\s*((\s\-?\d{1,3})*,\d{2})$");

            return match.Success
                ? new Trasaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = -decimal.Parse(match.Groups[3].Value),
                    Description = match.Groups[2].Value.Trim()
                }
                : null;
        }

        private Trasaction ReadCurrencyTransaction(IEnumerator<string> input)
        {
            var match = Regex.Match(input.Current, @"^(\d{2}\.\d{2}\.\d{4})\s*([^\s].*[^\s])$");

            if (match.Success && input.MoveNext())
            {
                var match2 = Regex.Match(input.Current,
                    @"^\s*([\d\s]+\,\d+)\s(\w+)\s\/\sKurs\s([\d\,]+)\s+([\d\s]+\,\d+).*$");
                if (match2.Success)
                {
                    var trans = new Trasaction
                    {
                        TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                        RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                        Description = match.Groups[2].Value,
                        Amount = -decimal.Parse(match2.Groups[4].Value),
                        Currency = match2.Groups[2].Value,
                        CurAmount = -decimal.Parse(match2.Groups[1].Value)
                    };
                    return trans;
                }
            }

            return null;
        }
    }
}