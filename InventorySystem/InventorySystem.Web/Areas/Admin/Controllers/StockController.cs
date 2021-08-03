using InventorySystem.Web.Areas.Admin.Models;
using InventorySystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace InventorySystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new StockListModel();
            return View(model);
        }

        public JsonResult GetStockData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new StockListModel();
            var data = model.GetStocks(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateStockModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateStock();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create product");
                    _logger.LogError(ex, "Create product Failed");
                }
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditStockModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditStockModel model)
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
            var model = new StockListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}


