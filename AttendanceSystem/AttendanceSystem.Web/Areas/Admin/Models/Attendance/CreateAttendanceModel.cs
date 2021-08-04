using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Services;
using AttendanceSystem.Common.Utilities;
using Autofac;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class CreateAttendanceModel
    {
        [Required, Range(1, 100000)]
        public int StudentId { get; set; }

        public DateTime Date { get; set; }

        private readonly IAttendanceService _attendanceService;
        public CreateAttendanceModel()
        {
            _attendanceService = Startup.AutofacContainer.Resolve<IAttendanceService>();
        }

        public CreateAttendanceModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        internal void CreateAttendance()
        {
            var attendance = new Attendance
            {
                StudentId = StudentId,
                Date = Date
            };

            _attendanceService.CreateAttendance(attendance);
        }
    }
}
