using System;

namespace AdoNetExamples
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var student = new Student
            {
                Id = 1,
                Name = "Abdul Hamid",
                Weight = 5.5m
            };

            var myorm = new MyORM<Student>("Server=DESKTOP-9BILDI2\\SQLEXPRESS;Database=aspnetB5;User Id = aspnetb5; Password=Emon1122;");
            myorm.Insert(student);
        }
    }
}
