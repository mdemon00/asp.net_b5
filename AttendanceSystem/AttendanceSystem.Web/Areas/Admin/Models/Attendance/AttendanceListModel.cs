using AttendanceSystem.Attending.Services;
using AttendanceSystem.Web.Models;
using Autofac;
using System.Linq;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class AttendanceListModel
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceListModel()
        {
            _attendanceService = Startup.AutofacContainer.Resolve<IAttendanceService>();
        }

        public AttendanceListModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        internal object GetAttendances(DataTablesAjaxRequestModel dataTableModel)
        {
            var data = _attendanceService.GetAttendances(dataTableModel.PageIndex,
            dataTableModel.PageSize,
            dataTableModel.SearchText,
            dataTableModel.GetSortText(new string[] { "StudentId", "Date" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.StudentId.ToString(),
                                record.Date.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _attendanceService.DeleteAttendance(id);
        }
    }
}
