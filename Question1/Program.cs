using System;
using System.Collections.Generic;
using System.IO;

namespace Question1
{
    public class FileProcessor
    {
        static IList<string> ReadFiles(IList<string> fileNames)
        {
            foreach( var item in fileNames)
            {
                var textfile = File.ReadAllText(item);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
