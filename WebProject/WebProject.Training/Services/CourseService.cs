﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebProject.Common;
using WebProject.Training.BuisnessObjects;
using WebProject.Training.UnitOfWorks;

namespace WebProject.Training
{
    public class CourseService : ICourseService
    {
        private readonly ITrainingUnitOfWork _trainingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        public CourseService(ITrainingUnitOfWork trainingUnitOfWork, IDateTimeUtility dateTimeUtility)
        {
            _trainingUnitOfWork = trainingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
        }
        public IList<Course> GetAllCourses()
        {
            var courseEntities = _trainingUnitOfWork.Courses.GetAll();
            var courses = new List<Course>();

            foreach(var entity in courseEntities)
            {
                var course = new Course
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Fees = entity.Fees,
                    StartDate = entity.StartDate
                };

                courses.Add(course);
            }
            return courses;
        }

        public void CreateCourse(Course course)
        {
            if (course == null)
                throw new InvalidParameterException("Course was not provided");

            if (IsTitleAlreadyUsed(course.Title))
                throw new DuplicateTitleException("Course Title already exists");

            if(!IsValidStartDate(course.StartDate))
                throw new InvalidOperationException("Start date should be atleast 30 days ahead");


                _trainingUnitOfWork.Courses.Add(
                new Entities.Course
                {
                    Title = course.Title,
                    Fees = course.Fees,
                    StartDate = course.StartDate
                });

                _trainingUnitOfWork.Save();

        }

        public void EnrollStudent(Course course, Student student)
        {
            var courseEntity = _trainingUnitOfWork.Courses.GetById(course.Id);

            if (courseEntity == null)
                throw new InvalidOperationException("Course was not found");

            if (courseEntity.EnrolledStudents == null)
                courseEntity.EnrolledStudents.Add(new Entities.CourseStudents
                {
                    Student = new Entities.Student
                    {
                        Name = student.Name,
                        DateOfBirth = student.DateofBirth
                    }
                });
            _trainingUnitOfWork.Save();
        }

        private bool IsTitleAlreadyUsed(string title) =>
              _trainingUnitOfWork.Courses.GetCount(x => x.Title == title) > 0;

        private bool IsTitleAlreadyUsed(string title, int id) =>
      _trainingUnitOfWork.Courses.GetCount(x => x.Title == title && x.Id !=id) > 0;

        private bool IsValidStartDate(DateTime startDate) =>
            startDate.Subtract(_dateTimeUtility.Now).TotalDays > 30;

        public (IList<Course> records, int total, int totalDisplay) GetCourses(int pageIndex, int pageSize, string searchText, string sorttext)
        {
            var courseData =_trainingUnitOfWork.Courses.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Title.Contains(searchText), sorttext, string.Empty, pageIndex, pageSize);

            var resultData = (from course in courseData.data
                          select new Course
                          {
                              Id = course.Id,
                              Title = course.Title,
                              Fees = course.Fees,
                              StartDate = course.StartDate
                          }).ToList();
            return (resultData, courseData.total, courseData.totalDisplay);
        }

        public Course GetCourse(int id)
        {
            var course = _trainingUnitOfWork.Courses.GetById(id);

            if (course == null) return null;
            return new Course
            {
                Id = course.Id,
                Title = course.Title,
                Fees = course.Fees,
                StartDate = course.StartDate
            };
        }

        public void UpdateCourse(Course course)
        {
            if (course == null)
                throw new InvalidOperationException("Course is missing");

            if (IsTitleAlreadyUsed(course.Title, course.Id))
                throw new DuplicateTitleException("Course title already used in other course");

            var courseEntity = _trainingUnitOfWork.Courses.GetById(course.Id);

            if(courseEntity != null)
            {
                courseEntity.Title = course.Title;
                courseEntity.Fees = course.Fees;
                courseEntity.StartDate = course.StartDate;

                _trainingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find course");
        }

        public void DeleteCourse(int id)
        {
            _trainingUnitOfWork.Courses.Remove(id);
            _trainingUnitOfWork.Save();
        }
    }
}
