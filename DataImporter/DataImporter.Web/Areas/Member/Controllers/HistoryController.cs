using Autofac;
using DataImporter.Areas.Member.Models;
using DataImporter.Common.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "Member")]
    public class HistoryController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly ILifetimeScope _scope;

        public HistoryController(ILogger<GroupController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }
        public IActionResult Index()
        {
            var model = _scope.Resolve<HistoryListModel>();
            return View(model);
        }

        public JsonResult GetGroupData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<HistoryListModel>();
            var data = model.GetHistories(dataTablesModel);
            return Json(data);
        }
    }
}
