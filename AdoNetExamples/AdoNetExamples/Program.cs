using System;
using System.Collections.Generic;

namespace AdoNetExamples
{
    class House : IData
    {
        public int Id { get; set; }
        public Room Room { get; set; }
    }

    class Room : IData
    {
        public int Id { get; set; }
        public double Rent { get; set; }
        public Door Door { get; set; }
        public Window Window { get; set; }
    }
    class Door : IData
    {
        public int Id { get; set; }
        public double Height { get; set; }
        public Camera Camera { get; set; }
    }

    class Window : IData
    {
        public int Id { get; set; }
        public double size { get; set; }
    }

    class Camera : IData
    {
        public int Id { get; set; }
        public int CameraPX { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //var student = new Student
            //{
            //    Id = 1,
            //    Name = "Habib",
            //    Weight = 80.5m
            //};

            var house = new House
            {
                Id = 1,
                Room = new Room
                {
                    Id = 2,
                    Rent = 5879,
                    Door = new Door
                    {
                        Id = 3,
                        Height = 10.5,
                        Camera = new Camera
                        {
                            Id = 7,
                            CameraPX = 5
                        }
                    },
                    Window = new Window
                    {
                        Id = 5,
                        size = 55.5
                    }
                }
            };

            var myorm = new MyORM<House>("Server=DESKTOP-9BILDI2\\SQLEXPRESS;Database=aspnetB5;User Id = aspnetb5; Password=Emon1122;");
            //myorm.Insert(student);
            //myorm.Update(student);
            //myorm.Delete(student);
            //myorm.Delete(1);

            //var students = myorm.GetAll();
            //foreach (var person in students)
            //{
            //    Console.WriteLine($"Id: {person.Id} ");
            //    Console.WriteLine($"Name: {person.Name} ");
            //    Console.WriteLine($"Weight: {person.Weight} ");
            //}

            //var single_student = myorm.GetById(1);
            //Console.WriteLine($"Id: {single_student.Id} ");
            //Console.WriteLine($"Name: {single_student.Name} ");
            //Console.WriteLine($"Weight: {single_student.Weight} ");

            myorm.Insert(house);
        }
    }
}
