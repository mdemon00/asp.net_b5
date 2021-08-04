using AttendanceSystem.Attending.BuisnessObjects;
using System.Collections.Generic;

namespace AttendanceSystem.Attending.Services
{
    public interface IStudentService
    {
        (IList<Student> records, int total, int totalDisplay) GetStudents(int pageIndex, int pageSize, string searchText, string sortText);
        void CreateStudent(Student student);
        Student GetStudent(int id);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}
