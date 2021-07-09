using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Training;
using Autofac;
using WebProject.Training.BuisnessObjects;

namespace WebProject.Areas.Admin.Models
{
    public class EnrollStudentModel
    {
        private int studentId { get; set; }
        private string courseName { get; set; }

        private readonly ICourseService _courseService;
        public EnrollStudentModel()
        {
            _courseService = Startup.AutofacContainer.Resolve<ICourseService>();
        }

        public EnrollStudentModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public void EnrollStudent()
        {
            var courses = _courseService.GetAllCourses();

            var selectedCourse = courses.Where(x => x.Title == courseName).FirstOrDefault();

            var student = new Student
            {
                Id = studentId,
                DateofBirth = DateTime.Now,
                Name = "Jalaluddin",
            };

            _courseService.EnrollStudent(selectedCourse, student);
        }
    }
}
