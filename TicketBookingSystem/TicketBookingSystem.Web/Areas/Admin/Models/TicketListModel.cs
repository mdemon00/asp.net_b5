using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBookingSystem.Booking.Services;
using TicketBookingSystem.Web.Models;

namespace TicketBookingSystem.Web.Areas.Admin.Models
{
    public class TicketListModel
    {
        private readonly ITicketService _ticketService;
        public TicketListModel()
        {
            _ticketService = Startup.AutofacContainer.Resolve<ITicketService>();
        }

        public TicketListModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        internal object GetTickets(DataTablesAjaxRequestModel dataTableModel)
        {
            var data = _ticketService.GetTickets(dataTableModel.PageIndex,
            dataTableModel.PageSize,
            dataTableModel.SearchText,
            dataTableModel.GetSortText(new string[] { "CustomerId", "Destination", "TicketFee" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.CustomerId.ToString(),
                                record.Destination,
                                record.TicketFee.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _ticketService.DeleteTicket(id);
        }
    }
}
