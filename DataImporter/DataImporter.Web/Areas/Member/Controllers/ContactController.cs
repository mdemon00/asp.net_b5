using DataImporter.Areas.Member.Models;
using DataImporter.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
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
            var model = new ContactListModel();
            var data = model.GetColums();

            return Json(data);
        }
    }
}
