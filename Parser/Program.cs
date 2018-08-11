using System;
using System.IO;
using System.Linq;
using Core;

namespace Parser
{
    internal class Program
    {
        // List of "kontoutskrifte" we recognize
        private static readonly Type[] ParserClasses =
        {
            typeof(TrumfVisa),
            typeof(KomplettKreditt)
        };


        private static void Main(string[] args)
        {
            var filename = args[0];
            var content = File.ReadAllLines(filename);

            var result = ParserClasses
                .Select(t => (IParser) Activator.CreateInstance(t, new object[] {content}))
                .FirstOrDefault(p => p.IsParseable)?
                .GeTrasactions();

            foreach (var r in result) Console.WriteLine(r);
        }
    }
}