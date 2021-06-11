using System;

namespace Reflection
{
    public class Initializer1
    {
        private int _a;
        public Initializer1(int a)
        {
            _a = a;
        }

        public void InitStartup()
        {
            Console.WriteLine($"Starting initializer in setup 1, value of a: {_a}");
        }
    }
}
