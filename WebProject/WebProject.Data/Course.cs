using System;
using System.Collections.Generic;

namespace WebProject.Data
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Fees { get; set; }
        public DateTime StartDate { get; set; }
        List<Topic> Topics { get; set; }
    }
}
