using DataImporter.Areas.Member.Models;
using DataImporter.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;

        public GroupController(ILogger<GroupController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new GroupListModel();
            return View(model);
        }

        public JsonResult GetGroupData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new GroupListModel();
            var data = model.GetGroups(dataTablesModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            var model = new CreateGroupModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateGroupModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateGroup();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Create Group Failed");
                }
                
            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            else
            {
                return Ok();
            }
        }

        public IActionResult Edit(int id)
        {
            var model = new EditGroupModel();

            if (ModelState.IsValid)
            {
                try
                {
                    model.LoadModelData(id);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Group Id not found");
                }

            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            else
            {
                return Ok(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditGroupModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    model.Update();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Edit Group Failed");
                }

            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            else
            {
                return Ok();
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = new GroupListModel();

            if (ModelState.IsValid)
            {
                try
                {
                    model.Delete(id);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Delete Group Failed");
                }

            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            else
            {
                return Ok();
            }
        }
    }
}
