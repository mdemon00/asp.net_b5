using AttendanceSystem.Attending.BuisnessObjects;
using System.Collections.Generic;

namespace AttendanceSystem.Attending.Services
{
    public interface IAttendanceService
    {
        (IList<Attendance> records, int total, int totalDisplay) GetAttendances(int pageIndex, int pageSize, string searchText, string sortText);
        void CreateAttendance(Attendance student);
        Attendance GetAttendance(int id);
        void UpdateAttendance(Attendance student);
        void DeleteAttendance(int id);
    }
}
