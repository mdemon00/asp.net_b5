using AttendanceSystem.Attending.BuisnessObjects;
using AttendanceSystem.Attending.Exceptions;
using AttendanceSystem.Attending.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceSystem.Attending.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAttendingUnitOfWork _attendingUnitOfWork;

        public StudentService(IAttendingUnitOfWork attendingUnitOfWork)
        {
            _attendingUnitOfWork = attendingUnitOfWork;
        }

        public void CreateStudent(Student student)
        {
            if (student == null)
                throw new InvalidParameterException("Student was not provided");

            _attendingUnitOfWork.Students.Add(new Entites.Student
            {
                Name = student.Name,
                StudentRollNumber = student.StudentRollNumber
            });

            _attendingUnitOfWork.Save();
        }

        public void DeleteStudent(int id)
        {
            _attendingUnitOfWork.Students.Remove(id);
            _attendingUnitOfWork.Save();
        }

        public Student GetStudent(int id)
        {
            var student = _attendingUnitOfWork.Students.GetById(id);

            if (student == null) return null;

            return new Student
            {
                Id = student.Id,
                Name = student.Name,
                StudentRollNumber = student.StudentRollNumber
            };
        }

        public (IList<Student> records, int total, int totalDisplay) GetStudents(int pageIndex, int pageSize, string searchText, string sortText)
        {

            var studentData = _attendingUnitOfWork.Students.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                           sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from student in studentData.data
                              select new Student
                              {
                                  Id = student.Id,
                                  Name = student.Name,
                                  StudentRollNumber = student.StudentRollNumber
                              }).ToList();
            return (resultData, studentData.total, studentData.totalDisplay);
        }

        public void UpdateStudent(Student student)
        {
            if (student == null)
                throw new InvalidOperationException("Student is missing");

            var studentEntity = _attendingUnitOfWork.Students.GetById(student.Id);

            if (studentEntity != null)
            {
                studentEntity.Name = student.Name;
                studentEntity.StudentRollNumber = student.StudentRollNumber;

                _attendingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find student");
        }
    }
}
