using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Exceptions;
using AttendanceSystem.Attending.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceSystem.Attending.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendingUnitOfWork _attendingUnitOfWork;

        public AttendanceService(IAttendingUnitOfWork attendingUnitOfWork)
        {
            _attendingUnitOfWork = attendingUnitOfWork;
        }

        public void CreateAttendance(Attendance attendance)
        {
            if (attendance == null)
                throw new InvalidParameterException("Attendance was not provided");

            _attendingUnitOfWork.Attendances.Add(new Entites.Attendance
            {
                StudentId = attendance.StudentId,
                Date = attendance.Date
            });

            _attendingUnitOfWork.Save();
        }

        public void DeleteAttendance(int id)
        {
            _attendingUnitOfWork.Attendances.Remove(id);
            _attendingUnitOfWork.Save();
        }

        public Attendance GetAttendance(int id)
        {
            var attendance = _attendingUnitOfWork.Attendances.GetById(id);

            if (attendance == null) return null;

            return new Attendance
            {
                Id = attendance.Id,
                StudentId = attendance.StudentId,
                Date = attendance.Date
            };
        }

        public (IList<Attendance> records, int total, int totalDisplay) GetAttendances(int pageIndex, int pageSize, string searchText, string sortText)
        {

            int value = 0;
            int.TryParse(searchText, out value); // determine whether a string represents a numeric value

            var attendanceData = _attendingUnitOfWork.Attendances.GetDynamic(value == 0 ? null : x => x.StudentId.ToString().Contains(value.ToString()),
            sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from attendance in attendanceData.data
                              select new Attendance
                              {
                                  Id = attendance.Id,
                                  StudentId = attendance.StudentId,
                                  Date = attendance.Date
                              }).ToList();
            return (resultData, attendanceData.total, attendanceData.totalDisplay);
        }

        public void UpdateAttendance(Attendance attendance)
        {
            if (attendance == null)
                throw new InvalidOperationException("Attendance is missing");

            var attendanceEntity = _attendingUnitOfWork.Attendances.GetById(attendance.Id);

            if (attendanceEntity != null)
            {
                attendanceEntity.StudentId = attendance.StudentId;
                attendanceEntity.Date = attendance.Date;

                _attendingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find attendance");
        }
    }
}
