using AttendanceSystem.Attending.Entites;
using AttendanceSystem.Data;

namespace AttendanceSystem.Attending.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance, int>
    {
    }
}
