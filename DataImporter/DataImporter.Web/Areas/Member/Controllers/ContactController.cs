using DataImporter.Areas.Member.Models;
using DataImporter.Common.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private IWebHostEnvironment _environment;

        public ContactController(ILogger<ContactController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            //var model = new CreateContactModel();
            //model.Create();

            var model = new ContactListModel();
            return View(model);
        }

        public JsonResult GetContactsData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new ContactListModel();
            var data = model.GetContacts(dataTablesModel);
            return Json(data);
        } 
        
        public JsonResult GetColumns()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new ContactListModel();
            var data = model.GetColums(dataTablesModel);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (file.Length > 0)
            {
                try
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest("File couldn't upload." + ex);
                }

            }
            return Ok();
        }

        public IActionResult Import(ImportContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    var fullDir = Path.Combine(uploads, model.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(fullDir);

                    model.Import(fullDir,fileName, model.GroupName);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Import Failed");
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
                return Ok(model.GroupName);
            }
        }

    }
}
