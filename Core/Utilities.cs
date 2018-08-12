using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core
{
    public class Utilities
    {
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
    }
}