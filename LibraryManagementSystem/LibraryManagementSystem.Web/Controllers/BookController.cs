using LibraryManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LibraryManagementSystem.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new BookListModel();
            return View(model);
        }

        public JsonResult GetBookData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new BookListModel();
            var data = model.GetBooks(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateBookModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateBook();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create book");
                    _logger.LogError(ex, "Create book Failed");
                }
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditBookModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditBookModel model)
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
            var model = new BookListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
