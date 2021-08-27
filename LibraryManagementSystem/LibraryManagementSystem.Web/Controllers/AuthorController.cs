using LibraryManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LibraryManagementSystem.Web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ILogger<AuthorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new AuthorListModel();
            return View(model);
        }

        public JsonResult GetAuthorData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new AuthorListModel();
            var data = model.GetAuthors(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateAuthorModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateAuthor();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create author");
                    _logger.LogError(ex, "Create author Failed");
                }
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditAuthorModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditAuthorModel model)
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
            var model = new AuthorListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
