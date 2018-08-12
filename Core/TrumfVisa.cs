using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NodaTime;
using NodaTime.Text;

namespace Core
{
    public class TrumfVisa : ParserBase
    {
        // A text that only apprears in an Trumf Visa invoice
        private readonly string MagicText = "Trumf Visa";

        private static readonly NodaTime.Text.LocalDatePattern
            DatePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yy");


        public TrumfVisa(string[] content)
            : base(content)
        {
        }

        public override bool IsParseable => Contains(MagicText);

        public override IEnumerable<Trasaction> GetTransactions()
        {
            var pattern =
                @"^\s*(\d{2})\.(\d{2})\.(\d{2})\s*([^\s].*[^\s])\s*(\d{2})\.(\d{2})\s*\d{2}\.\d{2}\.\d{2}\s([\d\s]+\,\d\d)\s*$";
            var inpattern =
                @"^\s*(\d{2}\.\d{2}\.\d{2})\s*([^\s].*[^\s])\s*(\d{2}\.\d{2}\.\d{2})\s+([\d\s]+\,\d\d)\s*$";
            var curpattern = @"^\s*([^\s]+)\s+([^\s]+)\s+Kurs\s+([^\s]+)\s*$";
            var list = new List<Trasaction>();

            foreach (var line in Content)
            {
                var z = Regex.Match(line, pattern);

                if (z.Success)
                {
                    var recordDate = new LocalDate(
                        2000 + int.Parse(z.Groups[3].Value),
                        int.Parse(z.Groups[2].Value),
                        int.Parse(z.Groups[1].Value));

                    var transactionDate = new LocalDate(
                        2000 + int.Parse(z.Groups[3].Value),
                        int.Parse(z.Groups[6].Value),
                        int.Parse(z.Groups[5].Value));

                    if (transactionDate > recordDate)
                        transactionDate = new LocalDate(
                            recordDate.PlusMonths(-1).Year,
                            transactionDate.Month,
                            transactionDate.Day
                        );

                    list.Add(new Trasaction
                    {
                        RecordDate = recordDate,
                        TransactionDate = transactionDate,
                        Description = z.Groups[4].Value,
                        Amount = -decimal.Parse(z.Groups[7].Value)
                    });

                    continue;
                }

                z = Regex.Match(line, inpattern);
                if (z.Success)
                {
                    list.Add(new Trasaction
                    {
                        TransactionDate = DatePattern.Parse(z.Groups[3].Value).Value,
                        RecordDate = DatePattern.Parse(z.Groups[1].Value).Value,
                        Amount = decimal.Parse(z.Groups[4].Value),
                        Description = z.Groups[2].Value
                    });

                    continue;
                }

                z = Regex.Match(line, curpattern);
                if (z.Success)
                {
                    var transaction = list.LastOrDefault();
                    Debug.Assert(transaction != null, nameof(transaction) + " != null");
                    transaction.Currency = z.Groups[1].Value;

                    transaction.CurAmount = -decimal.Parse(z.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            return list;
        }
    }
}