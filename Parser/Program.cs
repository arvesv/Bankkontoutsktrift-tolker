using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;

namespace Parser
{
    class Program
    {
        // List of "kontoutskrifte" we recognize
        private static readonly Type[] ParserClasses =
        {
            typeof(Core.TrumfVisa)
        };



        static void Main(string[] args)
        {
            var filename = args[0];
            var content = File.ReadAllLines(filename);

            var result = (ParserClasses
                .Select(t => (IParser) Activator.CreateInstance(t, new object[] {content}))
                .FirstOrDefault(p => p.IsParseable))?
                .GeTrasactions();

            foreach (var r in result)
            {
                Console.Write(r);
            }



        }
    }
}
