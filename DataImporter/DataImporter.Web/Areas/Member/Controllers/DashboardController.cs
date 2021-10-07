using Autofac;
using DataImporter.Areas.Member.Models;
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
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ILifetimeScope _scope;
        public DashboardController(ILogger<DashboardController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }
        public IActionResult Index()
        {
            var model = _scope.Resolve<DashboardModel>();

            model.GetGroupCount();
            model.GetPendingCount();
            model.GetImportCount();
            model.GetExportCount();

            return View(model);
        }
    }
}
