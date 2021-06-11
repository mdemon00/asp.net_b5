using System;
using System.Collections.Generic;
using System.Linq;

namespace Question4
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // When you write code, you should assign values to these lists
            List<Student> list1 = new List<Student> { new Student { Name = "Habib", Age = 22 }, new Student { Name = "Rahim", Age = 19 } };
            List<Student> list2 = new List<Student> { new Student { Name = "Asif", Age = 24 }, new Student { Name = "Bimol", Age = 21 } };

            List<string> result = (List<string>)(from student in list1.Union(list2)
                                                 orderby student.Name, student.Age
                                                 select student.Name);
        }
    }
}
