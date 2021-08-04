
using AttendanceSystem.Data;
using System;

namespace AttendanceSystem.Attending.Entites
{
    public class Attendance : IEntity<int>
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
    }
}
