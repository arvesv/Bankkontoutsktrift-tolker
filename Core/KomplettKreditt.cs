using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using NodaTime;
using NodaTime.Text;

namespace Core
{
    public class KomplettKreditt : ParserBase
    {
        // A text that only apprears in an Komplett Kredittkort invoice
        private readonly string MagicText = "Komplett Bank MasterCard";

        public KomplettKreditt(string[] content)
            : base(content)
        {
        }

        public override bool IsParseable => Contains(MagicText);

        public override IEnumerable<Trasaction> GeTrasactions()
        {
            foreach (var line in _content)
            {
                Trasaction trans = ReadSingleTransaction(line);
                if (trans != null)
                {
                    yield return trans;
                }
                
            }
        }

        private Trasaction ReadSingleTransaction(string line)
        {
            var pattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yyyy");
            var match = Regex.Match(line, @"^(\d{2}\.\d{2}\.\d{4})\s*([^\s].*[^\s])\s*\s{2}(\d[\d\s]*,\d{2})$");

            return match.Success ? new Trasaction
            {
                TransactionDate = pattern.Parse(match.Groups[1].Value).Value,
                Amount = -decimal.Parse(match.Groups[3].Value),
                Description = match.Groups[2].Value

            } : null;
        }
    }
}
