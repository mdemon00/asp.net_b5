using AttendanceSystem.Attending.Repositories;
using AttendanceSystem.Data;

namespace AttendanceSystem.Attending.UnitOfWorks
{
    public interface IAttendingUnitOfWork : IUnitOfWork
    {
        IStudentRepository Students { get; }
        IAttendanceRepository Attendances { get; }
    }
}
