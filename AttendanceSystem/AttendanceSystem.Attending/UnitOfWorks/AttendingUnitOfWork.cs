
using AttendanceSystem.Attending.Contexts;
using AttendanceSystem.Attending.Repositories;
using AttendanceSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Attending.UnitOfWorks
{
    public class AttendingUnitOfWork : UnitOfWork, IAttendingUnitOfWork
    {
        public IStudentRepository Students { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }

        public AttendingUnitOfWork(IAttendingContext context,
            IStudentRepository students, IAttendanceRepository attendances) : base((DbContext)context)
        {
            Students = students;
            Attendances = attendances;
        }
    }
}
