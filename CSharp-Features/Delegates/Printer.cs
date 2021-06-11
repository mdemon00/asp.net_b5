using System;

namespace Delegates
{
    public class Printer
    {
        public void PrintFromat1(string text)
        {
            Console.WriteLine($"__//{text}//__");
        }

        public void PrintFromat2(string text)
        {
            Console.WriteLine($"__$${text}$$__");
        }
    }
}
