using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Web.Areas.Admin.Models;
using SocialNetwork.Web.Models;
using System;

namespace SocialNetwork.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotoController : Controller
    {
        private readonly ILogger<PhotoController> _logger;

        public PhotoController(ILogger<PhotoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = new PhotoListModel();
            return View(model);
        }

        public JsonResult GetPhotoData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new PhotoListModel();
            var data = model.GetPhotos(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreatePhotoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatePhoto();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create photo");
                    _logger.LogError(ex, "Create Photo Failed");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditPhotoModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditPhotoModel model)
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
            var model = new PhotoListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
