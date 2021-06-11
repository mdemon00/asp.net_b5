using System;
using System.Collections.Generic;

namespace Question2
{
    public static class SmartSort
    {
        delegate int compare<T, T1>();
        public static IList<T> Sort<T>(this IList<T> elements, compare<T,T> compare)
        {
            throw new NotImplementedException();
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
