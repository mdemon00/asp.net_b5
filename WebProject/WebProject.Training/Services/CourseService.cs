using System.Collections.Generic;
using System.Linq;
using WebProject.Training.BuisnessObjects;
using WebProject.Training.Context;

namespace WebProject.Training
{
    public class CourseService : ICourseService
    {
        private readonly TrainingContext _trainingContext;
        public CourseService(TrainingContext trainingContext)
        {
            _trainingContext = trainingContext;
        }
        public IList<Course> GetAllCourses()
        {
            var courseEntities = _trainingContext.Courses.ToList();
            var courses = new List<Course>();

            foreach(var entity in courseEntities)
            {
                var course = new Course
                {
                    Title = entity.Title,
                    Fees = entity.Fees
                };

                courses.Add(course);
            }
            return courses;
        }
    }
}
