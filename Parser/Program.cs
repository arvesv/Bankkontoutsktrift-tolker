using System;
using Core;

namespace Parser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            var result = Utilities.Parse(args[0]);

            foreach (var r in result) Console.WriteLine(r);
        }
    }
}