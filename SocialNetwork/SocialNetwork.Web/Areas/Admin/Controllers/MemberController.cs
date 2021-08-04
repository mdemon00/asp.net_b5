using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Web.Areas.Admin.Models;
using SocialNetwork.Web.Models;
using System;

namespace SocialNetwork.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;

        public MemberController(ILogger<MemberController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = new MemberListModel();
            return View(model);
        }

        public JsonResult GetMemberData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new MemberListModel();
            var data = model.GetMembers(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateMemberModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateMember();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create member");
                    _logger.LogError(ex, "Create Member Failed");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditMemberModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditMemberModel model)
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
            var model = new MemberListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
