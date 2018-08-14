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


        public override (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            var transaction = ReadTransaction(enumerator.Current);
            if (transaction != null) return (transaction, false);

            transaction = ReadCurrencyTransaction(enumerator);
            if (transaction != null) return (transaction, false);

            return base.ParseLine(enumerator);
        }


        private Transaction ReadTransaction(string line)
        {
            var match = Regex.Match(line, @"^(\d{2}\.\d{2}\.\d{4})\s+(.*\D)\s*((\s\-?\d{1,3})*,\d{2})$");

            return match.Success
                ? new Transaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = -decimal.Parse(match.Groups[3].Value),
                    Description = match.Groups[2].Value.Trim()
                }
                : null;
        }

        private Transaction ReadCurrencyTransaction(IEnumerator<string> input)
        {
            var match = Regex.Match(input.Current, @"^(\d{2}\.\d{2}\.\d{4})\s*([^\s].*[^\s])$");

            if (match.Success && input.MoveNext())
            {
                var match2 = Regex.Match(input.Current,
                    @"^\s*([\d\s]+\,\d+)\s(\w+)\s\/\sKurs\s([\d\,]+)\s+([\d\s]+\,\d+).*$");

                var trans = new Transaction
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

            return null;
        }
    }
}