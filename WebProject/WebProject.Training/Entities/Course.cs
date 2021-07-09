using System;
using System.Collections.Generic;
using WebProject.Data;

namespace WebProject.Training.Entities
{
    public class Course : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Fees { get; set; }
        public DateTime StartDate { get; set; }
        public List<Topic> Topics { get; set; }
        public List<CourseStudents> EnrolledStudents { get; set; }
    }
}
