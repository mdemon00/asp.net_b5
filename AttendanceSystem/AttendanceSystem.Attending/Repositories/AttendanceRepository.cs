
using AttendanceSystem.Attending.Contexts;
using AttendanceSystem.Attending.Entites;
using AttendanceSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Attending.Repositories
{
    public class AttendanceRepository : Repository<Attendance, int>, IAttendanceRepository
    {
        public AttendanceRepository(IAttendingContext context) : base((DbContext)context)
        {

        }
    }
}
