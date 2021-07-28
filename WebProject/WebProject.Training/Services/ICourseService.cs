using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Training.BuisnessObjects;

namespace WebProject.Training
{
    public interface ICourseService
    {
        IList<Course> GetAllCourses();
        void EnrollStudent(Course course, Student student);
        void CreateCourse(Course course);
        (IList<Course> records, int total, int totalDisplay) GetCourses(int pageIndex, int pageSize, string searchText, string sorttext);
        Course GetCourse(int id);
        void UpdateCourse(Course course);
        void DeleteCourse(int id);
    }
}
