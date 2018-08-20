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
                .SelectMany(f => Utilities.Parse(f))
                .OrderBy(l => l.TransactionDate).ToList();

            var x = OtherParser.ReadDataxCsv(@"S:\FellesRegnskap\2018\Kontoutskrift\Kontoutdrag 17-08-2018.csv");

            var (fir, la) = Utilities.CompareListe(result, x);


            foreach (var r in fir) Console.WriteLine(r);

            Console.ReadLine();
        }
    }
}