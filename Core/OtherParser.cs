using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NodaTime.Text;

namespace Core
{
    public class OtherParser
    {
        private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = " "
        };

        private static decimal ParseAmount(string text)
        {
            return decimal.Parse(text.Replace("\"", ""), NumberFormat);
        }

        public static IList<Transaction> ReadDataxCsv(string file)
        {
            var datePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yyyy");

            return File
                .ReadAllLines(file)
                .Skip(1)
                .Select(line => line.Split(';'))
                .Select(elems =>
                    new Transaction
                    {
                        Description = elems[5].Replace("\"", ""),
                        Amount = ParseAmount(elems[6]) - ParseAmount(elems[7]),
                        TransactionDate = datePattern.Parse(elems[2].Replace("\"", "")).Value
                    }).ToArray();
        }
    }
}