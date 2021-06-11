using System;
using System.Reflection;

namespace Question3
{
    abstract class BaseModel
    {

    }

    class Test : BaseModel
    {

    }

    class Test1 : BaseModel
    {

    }

    class Program
    {
        static void Main(string[] args)
        {

         var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                Console.WriteLine(type);
            }
        }
    }
}
