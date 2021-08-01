using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TicketBookingSystem.Web.Areas.Admin.Models;
using TicketBookingSystem.Web.Models;

namespace TicketBookingSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly ILogger<TicketController> _logger;

        public TicketController(ILogger<TicketController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = new TicketListModel();
            return View(model);
        }

        public JsonResult GetTicketData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = new TicketListModel();
            var data = model.GetTickets(dataTableModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateTicketModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.CreateTicket();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create ticket");
                    _logger.LogError(ex, "Create Ticket Failed");
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = new EditTicketModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditTicketModel model)
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
            var model = new TicketListModel();

            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
