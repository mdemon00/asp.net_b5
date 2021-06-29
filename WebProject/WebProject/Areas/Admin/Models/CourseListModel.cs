using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Training;
using Autofac;
using WebProject.Training.BuisnessObjects;

namespace WebProject.Areas.Admin.Models
{
    public class CourseListModel
    {
        private ICourseService _courseService;
        public IList<Course> Courses { get; set; }
        public CourseListModel()
        {
            _courseService = Startup.AutofacContainer.Resolve<ICourseService>();
        }
        public CourseListModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public void LoadModelData()
        {
            Courses = _courseService.GetAllCourses();
        }
    }
}
 