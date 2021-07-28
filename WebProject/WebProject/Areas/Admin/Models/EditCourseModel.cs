using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Training;
using Autofac;
using WebProject.Training.BuisnessObjects;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Areas.Admin.Models
{
    public class EditCourseModel
    {
        public int? Id { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Ttile should be less than 200 charcters")]
        public string Title { get; set; }
        [Required, Range(100, 50000)]
        public int? Fees { get; set; }
        [Required, Range(typeof(DateTime),"1/1/1971","12/12/2030")]
        public DateTime? StartDate { get; set; }
        private ICourseService _courseService;
        public EditCourseModel()
        {
            _courseService = Startup.AutofacContainer.Resolve<ICourseService>();
        }
        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public void LoadModelData(int id)
        {
            var course = _courseService.GetCourse(id);
            Id = course?.Id;
            Title = course?.Title;
            Fees = course?.Fees;
            StartDate = course?.StartDate;
        }

        internal void Update()
        {
            var course = new Course
            {
                Id = Id.HasValue? Id.Value : 0,
                Fees = Fees.HasValue? Fees.Value : 0,
                Title = Title,
                StartDate = StartDate.HasValue ? StartDate.Value : DateTime.MinValue
            };

            _courseService.UpdateCourse(course);
        }
    }
}
