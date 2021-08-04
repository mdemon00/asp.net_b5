
using AttendanceSystem.Data;

namespace AttendanceSystem.Attending.Entites
{
    public class Student : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentRollNumber { get; set; }
    }
}
