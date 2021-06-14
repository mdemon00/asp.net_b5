using System;
using System.Collections.Generic;

namespace AdoNetExamples
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var student = new Student
            {
                Id = 1,
                Name = "Habib",
                Weight = 80.5m
            };

            var myorm = new MyORM<Student>("Server=DESKTOP-9BILDI2\\SQLEXPRESS;Database=aspnetB5;User Id = aspnetb5; Password=Emon1122;");
            //myorm.Insert(student);
            //myorm.Update(student);
            //myorm.Delete(student);
            //myorm.Delete(1);

            var students = myorm.GetAll();
            foreach (var person in students)
            {
                Console.WriteLine($"Id: {person.Id} ");
                Console.WriteLine($"Name: {person.Name} ");
                Console.WriteLine($"Weight: {person.Weight} ");
            }

            //var single_student = myorm.GetById(1);
            //Console.WriteLine($"Id: {single_student.Id} ");
            //Console.WriteLine($"Name: {single_student.Name} ");
            //Console.WriteLine($"Weight: {single_student.Weight} ");
        }
    }
}
