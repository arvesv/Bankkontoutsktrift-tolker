using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class Utilities
    {
        public delegate bool Equals<T>(T t1, T t2);

        public static IEnumerable<string> PdfToText(string pdfFile)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "pdftotext",
                    RedirectStandardOutput = true,
                    Arguments = string.Format(@"-raw ""{0}"" -", pdfFile)
                }
            };

            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result.Split(Environment.NewLine);
        }


        public static IEnumerable<Transaction> Parse(string filename)
        {
            return Parse(PdfToText(filename), filename);
        }

        public static IEnumerable<Transaction> Parse(IEnumerable<string> content, string filename = null)
        {
            // Find all classes in this assembly implementing IParser, insansiate the class,
            // call IsParseable on it, and if yes return the parsed transactions.
            return typeof(Utilities).Assembly.GetTypes()
                       .Where(t => typeof(IParser).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                       .Select(t =>
                       {
                           var parser = (IParser) Activator.CreateInstance(t, content);
                           parser.Source = filename;
                           return parser;
                       })
                       .FirstOrDefault(p => p.IsParseable)?
                       .GetTransactions() ?? new List<Transaction>();
        }

        public static (IList<Transaction> onlyFirst, IList<Transaction> onlySecond) CompareListe(
            IList<Transaction> first, IList<Transaction> second)
        {
            IList<Transaction> onlyFirst = new List<Transaction>();
            bool[] alreadyMatchedList = new bool[second.Count];

            foreach (var firstLisTransaction in first)
            {
                int? bestMatch = null;
                var mindatediff = 0;


                for (int pos = 0; pos < second.Count; pos++)
                {
                    if (alreadyMatchedList[pos] || firstLisTransaction.Amount != second[pos].Amount) continue;

                    var datediff = Math.Abs((firstLisTransaction.TransactionDate - second[pos].TransactionDate).Days);
                    if(datediff > 3)
                        continue;

                    if (bestMatch == null || datediff < mindatediff)
                    {
                        mindatediff = bestMatch ?? Math.Min(mindatediff, datediff) | datediff;
                        bestMatch = pos;
                    }
                }

                if (bestMatch.HasValue)
                {
                    alreadyMatchedList[bestMatch.Value] = true;
                }
                else
                {
                    onlyFirst.Add(firstLisTransaction);
                }
            }

            var onlySecond = Enumerable.Range(0, second.Count)
                .Where(pos => !alreadyMatchedList[pos])
                .Select(pos => second[pos])
                .ToList();

            return (onlyFirst, onlySecond);
        }
    }
}