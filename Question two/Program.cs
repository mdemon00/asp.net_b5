using System;
using System.Reflection;

namespace Question_two
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassWithPrivateMethod target = new ClassWithPrivateMethod();
            ClassWithProtectedMethod target1 = new ClassWithProtectedMethod();

            ReflectionUtility utility = new ReflectionUtility();
            utility.CallPrivateOrProtected(target, "Print", new object[] { "Hello World" });
            utility.CallPrivateOrProtected(target1, "Print", new object[] { "Hello World" });

        }
    }

    public class ReflectionUtility
    {
        public void CallPrivateOrProtected(object targetObject, string methodName, object[] args)
        {
            // You have to write code here

            Type magicType = targetObject.GetType();
            MethodInfo magicMethod = magicType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            magicMethod.Invoke(targetObject, args);

        }
    }

    public class ClassWithPrivateMethod
    {
        private void Print(string value)
        {
            Console.WriteLine(value);
        }
    }

    public class ClassWithProtectedMethod
    {
        protected void Print(string value)
        {
            Console.WriteLine(value);
        }
    }
}
