using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Services;
using Autofac;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class CreateStudentModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        [Required, Range(1, 100000)]
        public int StudentRollNumber { get; set; }

        private readonly IStudentService _studentService;
        public CreateStudentModel()
        {
            _studentService = Startup.AutofacContainer.Resolve<IStudentService>();
        }

        public CreateStudentModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        internal void CreateStudent()
        {
            var student = new Student
            {
                Name = Name,
                StudentRollNumber = StudentRollNumber
            };

            _studentService.CreateStudent(student);
        }
    }
}
