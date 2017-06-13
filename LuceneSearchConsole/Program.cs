using System;

namespace LuceneSearchConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string _query;
            bool continues = true;
            Searcher _searcher = new Searcher();
            while (continues)
            {
                  _query = Console.ReadLine();
                 continues = _searcher.Search(_query);
            }
        }
    }
}