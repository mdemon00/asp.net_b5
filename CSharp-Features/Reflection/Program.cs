using System;
using System.IO;
using System.Reflection;

namespace Reflection
{
    public class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public double Weight { get; set; }

        public bool IsAvailable()
        {
            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Type y = typeof(Product);

            foreach (var constuctor in y.GetConstructors())
            {
                Console.WriteLine(constuctor.Name);
            }

            foreach (var field in y.GetFields())
            {
                Console.WriteLine(field.Name);
            }

            foreach (var propety in y.GetProperties())
            {
                Console.WriteLine(propety.Name);
            }

            foreach (var method in y.GetMethods())
            {
                Console.WriteLine(method.Name);
            }

            var path = @"C:\Learning\asp.net_b5\CSharp-Features\Reflection\config.txt";
            var configText = File.ReadAllText(path);

            var initClassName = configText.Split('=')[1].Trim();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                if (type.Name == initClassName)
                {
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(int) });
                    var initializerInstance = constructor.Invoke(new object[] { 5 });

                    MethodInfo method = type.GetMethod("InitStartup");
                    method.Invoke(initializerInstance, new object[0]);
                }
            }
        }
    }
}
