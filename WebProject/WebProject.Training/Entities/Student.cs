using System;
using System.Collections.Generic;
using WebProject.Data;

namespace WebProject.Training.Entities
{
    public class Student : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<CourseStudents> EnrolledCourses { get; set; }
    }
}
