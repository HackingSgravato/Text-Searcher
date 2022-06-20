using TextSearcherN;
using System;
using System.Diagnostics;
using System.IO;

namespace TextSearcher_Test
{
    internal class Program
    {
        static void Main()
        {
            TextSearcher searcher = new TextSearcher();
            searcher.textToSearch = "acsfinder";
            searcher.baseDirectory = Path.Combine(Path.GetTempPath(), "dumps");
            searcher.search();


            Console.WriteLine("the search process is finished in " + searcher.elapsedMilliseconds + "ms.");

            Process.Start(searcher.resultFile);
        }
    }
}
