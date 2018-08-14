using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public class Utilities
    {
        // List of "kontoutskrifte" we recognize

        public static Type[] ParserClasses { get; } =
        {
            typeof(TrumfVisa),
            typeof(KomplettKreditt),
            typeof(NorwegianKortet)
        };


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
                .GetTransactions();
        }
    }
}