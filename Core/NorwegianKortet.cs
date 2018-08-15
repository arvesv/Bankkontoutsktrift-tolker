using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NodaTime.Text;

namespace Core
{
    public class NorwegianKortet : ParserBase
    {
        private static readonly LocalDatePattern
            DatePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yyyy");

        private readonly NumberFormatInfo _norNumberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = "."
        };

        public NorwegianKortet(IEnumerable<string> content) : base(content)
        {
        }


        public override bool IsParseable => Contains("Norwegian-kortet");

        public override (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            Transaction trans;

            trans = ParseTransaction(enumerator.Current);
            if (trans != null) return (trans, false);

            trans = ParseInputTransaction(enumerator.Current);
            if (trans != null) return (trans, false);

            return (null, false);
        }

        private Transaction ParseInputTransaction(string line)
        {
            var pattern = @"^(\d{2}\.\d{2}\.\d{4})\s+(.*)\s-\s(?<amount>-?\d{1,3}(\.\d{3})*\,\d{2})$";

            var match = Regex.Match(line, pattern);
            return match.Success
                ? new Transaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = -decimal.Parse(match.Groups["amount"].Value, _norNumberFormat),
                    Description = match.Groups[2].Value,
                    CurAmount = -decimal.Parse(match.Groups["amount"].Value, _norNumberFormat),
                    Currency = "NOK"
                }
                : null;

        }


        private Transaction ParseTransaction(string line)
        {
            var inncomePattern =
                @"^(\d{2}\.\d{2}\.\d{4})\s+(.*)\s\d{6}[\s\*]+\d{4}.*\s(-?\d{1,3}(\.\d{3})*\,\d{2})\s(?<currency>\w{3})\s((\d+\.\d+)\s)?(?<amount>-?\d{1,3}(\.\d{3})*\,\d{2})$";

            var match = Regex.Match(line, inncomePattern);
            return match.Success
                ? new Transaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = -decimal.Parse(match.Groups["amount"].Value, _norNumberFormat),
                    Description = match.Groups[2].Value,
                    CurAmount = -decimal.Parse(match.Groups[3].Value, _norNumberFormat),
                    Currency = match.Groups["currency"].Value
                }
                : null;
        }
    }
}