using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TicketBookingSystem.Web.Areas.Admin.Models;
using TicketBookingSystem.Web.Models;

namespace TicketBookingSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new CustomerListModel();
            return View(model);
        }

        public JsonResult GetCustomerData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new CustomerListModel();
            var data = model.GetCustomers(dataTableModel);
            return Json(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateCustomerModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateCustomer();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create customer");
                    _logger.LogError(ex, "Create Customer Failed");
                }
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditCustomerModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditCustomerModel model)
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
            var model = new CustomerListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
