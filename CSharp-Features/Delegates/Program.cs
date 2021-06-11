using System;

namespace Delegates
{
    class Program
    {
        public delegate void Perform(string text);
        static void Main(string[] args)
        {
            var printer = new Printer();
            var text = "hello world";

            Perform del1 = printer.PrintFromat1;
            Perform del2 = printer.PrintFromat2;

            ProcessText(text, del1);
            ProcessText(text, del2);
        }

        public static void ProcessText(string text, Perform perform)
        {
            perform(text);
        }
    }
}
