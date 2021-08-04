using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Services;
using AttendanceSystem.Common.Utilities;
using Autofac;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class EditAttendanceModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, Range(1, 100000)]
        public int? StudentId { get; set; }
        public DateTime Date { get; set; }

        private readonly IAttendanceService _attendanceService;
        public EditAttendanceModel()
        {
            _attendanceService = Startup.AutofacContainer.Resolve<IAttendanceService>();
        }

        public EditAttendanceModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public void LoadModelData(int id)
        {
            var attendance = _attendanceService.GetAttendance(id);
            StudentId = attendance.StudentId;
            Date = attendance.Date;
        }

        internal void Update()
        {
            var attendance = new Attendance
            {
                Id = Id.HasValue ? Id.Value : 0,
                StudentId = StudentId.HasValue ? StudentId.Value : 0,
                Date = Date
            };
            _attendanceService.UpdateAttendance(attendance);
        }
    }
}
