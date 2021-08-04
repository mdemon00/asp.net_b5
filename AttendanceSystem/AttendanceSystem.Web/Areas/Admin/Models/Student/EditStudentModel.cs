using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Services;
using Autofac;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Web.Areas.Admin.Models
{
    public class EditStudentModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }

        [Required, Range(1, 100000)]
        public int? StudentRollNumber { get; set; }

        private readonly IStudentService _studentService;
        public EditStudentModel()
        {
            _studentService = Startup.AutofacContainer.Resolve<IStudentService>();
        }

        public EditStudentModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void LoadModelData(int id)
        {
            var student = _studentService.GetStudent(id);
            Name = student.Name;
            StudentRollNumber = student?.StudentRollNumber;
        }

        internal void Update()
        {
            var student = new Student
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name,
                StudentRollNumber = StudentRollNumber.HasValue ? StudentRollNumber.Value : 0,
            };
            _studentService.UpdateStudent(student);
        }
    }
}
