using Autofac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketBookingSystem.Booking.BuisnessObjects;
using TicketBookingSystem.Booking.Services;

namespace TicketBookingSystem.Web.Areas.Admin.Models
{
    public class EditCustomerModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        [Required, Range(1, 150)]
        public int? Age { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Address should be less than 200 charcaters")]
        public string Address { get; set; }

        private readonly ICustomerService _customerService;

        public EditCustomerModel()
        {
            _customerService = Startup.AutofacContainer.Resolve<ICustomerService>();
        }

        public EditCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void LoadModelData(int id)
        {
            var customer = _customerService.GetCustomer(id);
            Id = customer?.Id;
            Name = customer?.Name;
            Age = customer?.Age;
            Address = customer?.Address;
        }

        internal void Update()
        {
            var customer = new Customer
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name,
                Age = Age.HasValue ? Age.Value : 0,
                Address = Address
            };
            _customerService.UpdateCustomer(customer);
        }
    }
}
