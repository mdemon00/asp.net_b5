using AttendanceSystem.Attending.Services;
using AttendanceSystem.Web.Models;
using Autofac;
using System.Linq;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class StudentListModel
    {
        private readonly IStudentService _studentService;
        public StudentListModel()
        {
            _studentService = Startup.AutofacContainer.Resolve<IStudentService>();
        }

        public StudentListModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        internal object GetStudents(DataTablesAjaxRequestModel dataTableModel)
        {
            var data = _studentService.GetStudents(dataTableModel.PageIndex,
            dataTableModel.PageSize,
            dataTableModel.SearchText,
            dataTableModel.GetSortText(new string[] { "Name", "StudentRollNumber" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name.ToString(),
                                record.StudentRollNumber.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _studentService.DeleteStudent(id);
        }
    }
}
