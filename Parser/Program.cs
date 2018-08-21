using System;
using System.IO;
using System.Linq;
using Core;

namespace Parser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Error.WriteLine("Path {0} does not exist. Please provide a path with bank statements", args[0]);
                return;
            }


            var result = Directory
                .GetFiles(directory, "*.pdf")
                .SelectMany(Utilities.Parse)
                .OrderBy(l => l.TransactionDate)
                .Where(l => l.TransactionDate.Year == 2018)
                .ToList();

            var x = OtherParser.ReadDataxCsv(@"S:\FellesRegnskap\2018\Kontoutskrift\datax.csv");

            var (fir, la) = Utilities.CompareListe(result, x);


            Console.WriteLine("First {0} of {1}", fir.Count, result.Count);
            foreach (var r in fir) Console.WriteLine(r);

            Console.WriteLine("\nSecond {0} of {1}", la.Count, x.Count);
            foreach (var r in la) Console.WriteLine(r);

            Console.ReadLine();
        }
    }
}