using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NodaTime;
using NodaTime.Text;

namespace Core
{
    public class TrumfVisa : ParserBase
    {
        private static readonly LocalDatePattern
            DatePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yy");

        private readonly NumberFormatInfo _trumfNumberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = " "
        };

        // A text that only apprears in an Trumf Visa invoice
        private readonly string MagicText = "Trumf Visa";


        public TrumfVisa(string[] content)
            : base(content)
        {
        }

        public override bool IsParseable => Contains(MagicText);

        public override (Transaction, bool) ParseLine(IEnumerator<string> enumerator)
        {
            var (trans, readheadvar) = ParseTransaction(enumerator);
            if (trans != null) return (trans, readheadvar);

            trans = ParseInTransaction(enumerator.Current);
            if (trans != null) return (trans, false);

            return base.ParseLine(enumerator);
        }


        private (Transaction transacion, bool haslookedhead) ParseTransaction(IEnumerator<string> enumerator)
        {
            var pattern =
                @"^\s*(\d{2}\.\d{2}\.\d{2})\s*([^\s].*[^\s])\s*(\d{2})\.(\d{2})\s*\d{2}\.\d{2}\.\d{2}\s([\d\s]+\,\d\d)\s*$";
            var currencyPattern = @"^\s*([^\s]+)\s+([^\s]+)\s+Kurs\s+([^\s]+)\s*$";


            var match = Regex.Match(enumerator.Current, pattern);
            if (match.Success)
            {
                var recordDate = DatePattern.Parse(match.Groups[1].Value).Value;
                var transactionDate = new LocalDate(
                    recordDate.Year,
                    int.Parse(match.Groups[4].Value),
                    int.Parse(match.Groups[3].Value));

                // If they are in different years.
                if (transactionDate > recordDate)
                    transactionDate = new LocalDate(
                        recordDate.PlusMonths(-1).Year,
                        transactionDate.Month,
                        transactionDate.Day);

                var trans = new Transaction
                {
                    RecordDate = recordDate,
                    TransactionDate = transactionDate,
                    Description = match.Groups[2].Value,
                    Amount = -decimal.Parse(
                        match.Groups[5].Value, _trumfNumberFormat)
                };

                var lookahead = false;

                // Lookahead for currency
                if (enumerator.MoveNext())
                {
                    match = Regex.Match(enumerator.Current, currencyPattern);
                    if (match.Success)
                    {
                        trans.Currency = match.Groups[1].Value;
                        trans.CurAmount = -decimal.Parse(match.Groups[2].Value,
                            CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else
                    {
                        lookahead = true;
                    }
                }

                return (trans, lookahead);
            }

            return (null, false);
        }

        private Transaction ParseInTransaction(string line)
        {
            var inncomePattern =
                @"^\s*(\d{2}\.\d{2}\.\d{2})\s*([^\s].*[^\s])\s*(\d{2}\.\d{2}\.\d{2})\s+([\d\s]+\,\d\d)\s*$";

            var match = Regex.Match(line, inncomePattern);
            return match.Success
                ? new Transaction
                {
                    TransactionDate = DatePattern.Parse(match.Groups[3].Value).Value,
                    RecordDate = DatePattern.Parse(match.Groups[1].Value).Value,
                    Amount = decimal.Parse(match.Groups[4].Value, _trumfNumberFormat),
                    Description = match.Groups[2].Value
                }
                : null;
        }
    }
}