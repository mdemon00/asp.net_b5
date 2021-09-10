using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
