
using AttendanceSystem.Attending.Contexts;
using AttendanceSystem.Attending.Entites;
using AttendanceSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Attending.Repositories
{
    public class StudentRepository : Repository<Student, int>, IStudentRepository
    {
        public StudentRepository(IAttendingContext context) : base((DbContext)context)
        {

        }
    }
}
