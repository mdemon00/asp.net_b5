using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Areas.Admin.Models;

namespace WebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            var model = new CourseListModel();
            model.LoadModelData();
            return View(model);
        }
    }
}
