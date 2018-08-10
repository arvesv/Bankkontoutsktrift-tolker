using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using NodaTime;

namespace Core
{
    public class TrumfVisa : IParser
    {
        private readonly string[] _content;

        public TrumfVisa(string[] content)
        {
            _content = content;
        }

        public IEnumerable<Trasaction> GeTrasactions()
        {
            string pattern = @"^\s*(\d{2})\.(\d{2})\.(\d{2}).*(\d{2})\.(\d{2})\s\d{2}\.\d{2}\.\d{2}\s([\d\s]+\,\d\d)\s*$";
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
                        int.Parse(z.Groups[5].Value),
                        int.Parse(z.Groups[4].Value));

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
                        Amount = decimal.Parse(z.Groups[6].Value)
                    });
                }
            }
            return list;
        }

        public bool IsParseable
        {

            get
            {
                return _content.FirstOrDefault(l => l.Contains("Trumf Visa")) != null;
            }
        }
    }
}