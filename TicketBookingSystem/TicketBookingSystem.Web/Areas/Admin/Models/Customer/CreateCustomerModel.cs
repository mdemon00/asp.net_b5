using Autofac;
using System;
using System.ComponentModel.DataAnnotations;
using TicketBookingSystem.Booking.BuisnessObjects;
using TicketBookingSystem.Booking.Services;

namespace TicketBookingSystem.Web.Areas.Admin.Models
{
    public class CreateCustomerModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        [Required, Range(1, 150)]
        public int Age { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Address should be less than 200 charcaters")]
        public string Address { get; set; }

        private readonly ICustomerService _customerService;

        public CreateCustomerModel()
        {
            _customerService = Startup.AutofacContainer.Resolve<ICustomerService>();
        }

        public CreateCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        internal void CreateCustomer()
        {
            var customer = new Customer
            {
                Name = Name,
                Age = Age,
                Address = Address
            };

            _customerService.CreateCustomer(customer);
        }
    }
}
