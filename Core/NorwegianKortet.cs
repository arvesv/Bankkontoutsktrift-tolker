using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NodaTime.Text;

namespace Core
{
    public class NorwegianKortet : ParserBase
    {
        public NorwegianKortet(IEnumerable<string> content) : base(content)
        {
        }

        private static readonly LocalDatePattern
            DatePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yyyy");

        private readonly NumberFormatInfo _norNumberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = "."
        };


        public override bool IsParseable => Contains("Norwegian-kortet");

        public override (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            Transaction trans;

            trans = ParseTransaction(enumerator.Current);
            if (trans != null)
            {
                return (trans, false);
            }

            return (null, false);
        }

        private Transaction ParseTransaction(string line)
        {
            var inncomePattern =
                @"^(\d{2}\.\d{2}\.\d{4})\s+(.*)\s\d{6}[\s\*]+\d{4}.*(\w{3})\s(-?\d{1,3}(\.\d{3})*\,\d{2})$";

            var match = Regex.Match(line, inncomePattern);
            return match.Success
                ? new Transaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = -decimal.Parse(match.Groups[4].Value, _norNumberFormat),
                    Description = match.Groups[2].Value
                }
                : null;
        }



    }
}