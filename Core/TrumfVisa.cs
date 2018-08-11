﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NodaTime;

namespace Core
{
    public class TrumfVisa : ParserBase
    {
        // A text that only apprears in an Trumf Visa invoice
        private readonly string MagicText = "Trumf Visa";
        public TrumfVisa(string[] content)
        :base(content)
        {
        }

        public override IEnumerable<Trasaction> GetTransactions()
        {
            var pattern =
                @"^\s*(\d{2})\.(\d{2})\.(\d{2})\s*([^\s].*[^\s])\s*(\d{2})\.(\d{2})\s*\d{2}\.\d{2}\.\d{2}\s([\d\s]+\,\d\d)\s*$";
            var inpattern =
                @"^\s*(\d{2})\.(\d{2})\.(\d{2})\s*([^\s].*[^\s])\s*(\d{2})\.(\d{2}).(\d{2})\s*([\d\s]+\,\d\d)\s*$";
            var curpattern = @"^\s*([^\s]+)\s+([^\s]+)\s+Kurs\s+([^\s]+)\s*$";
            var list = new List<Trasaction>();

            foreach (var line in _content)
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
                    var recordDate = new LocalDate(
                        2000 + int.Parse(z.Groups[3].Value),
                        int.Parse(z.Groups[2].Value),
                        int.Parse(z.Groups[1].Value));

                    list.Add(new Trasaction
                    {
                        TransactionDate = recordDate,
                        RecordDate = recordDate,
                        Amount = decimal.Parse(z.Groups[8].Value),
                        Description = z.Groups[4].Value
                    });

                    continue;
                }

                z = Regex.Match(line, curpattern);
                if (z.Success)
                {
                    var transaction = list.LastOrDefault();
                    Debug.Assert(transaction != null, nameof(transaction) + " != null");
                    transaction.Currency = z.Groups[1].Value;

                    transaction.CurAmount = -decimal.Parse(z.Groups[2].Value);
                }
            }

            return list;
        }

        public override bool IsParseable => Contains(MagicText);
    }
}