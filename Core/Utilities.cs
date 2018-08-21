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

        public static bool IsEqualTrans(Transaction t1, Transaction t2)
        {
            const int maxDateDiff = 4;

            return t1.Amount == t2.Amount && (t1.TransactionDate - t2.TransactionDate).Days < maxDateDiff;
        }

        private static IList<T> OnlyInFirstList<T>(IEnumerable<T> first, IList<T> second, Equals<T> comparer)
        {
            return (from t in first
                let potentialMatches =
                    second.Where(s => comparer(s, t))
                where !potentialMatches.Any()
                select t).ToList();
        }

        public static (IList<Transaction> onlyFirst, IList<Transaction> onlySecond) CompareListe(
            IList<Transaction> first, IList<Transaction> second)
        {
            return (
                OnlyInFirstList(first, second, IsEqualTrans),
                OnlyInFirstList(second, first, IsEqualTrans));
        }
    }
}