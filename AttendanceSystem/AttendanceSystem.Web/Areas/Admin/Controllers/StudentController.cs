using AttendanceSystem.Web.Areas.Admin.Models;
using AttendanceSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AttendanceSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = new StudentListModel();
            return View(model);
        }

        public JsonResult GetStudentData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new StudentListModel();
            var data = model.GetStudents(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateStudentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateStudent();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create student");
                    _logger.LogError(ex, "Create Student Failed");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditStudentModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditStudentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Update();
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var model = new StudentListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
