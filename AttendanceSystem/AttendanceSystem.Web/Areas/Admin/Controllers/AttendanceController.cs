using AttendanceSystem.Web.Areas.Admin.Models;
using AttendanceSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AttendanceSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AttendanceController : Controller
    {
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(ILogger<AttendanceController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = new AttendanceListModel();
            return View(model);
        }

        public JsonResult GetAttendanceData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new AttendanceListModel();
            var data = model.GetAttendances(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateAttendanceModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateAttendance();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create student");
                    _logger.LogError(ex, "Create Attendance Failed");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditAttendanceModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditAttendanceModel model)
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
            var model = new AttendanceListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

