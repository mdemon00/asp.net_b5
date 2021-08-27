using Autofac;
using LibraryManagementSystem.Reading.Services;
using LibraryManagementSystem.Reading.BuisnessObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.Models
{
    public class CreateBookModel
    {
        [Required, MaxLength(50, ErrorMessage = "Title should be less than 50 charcaters")]
        public string Title { get; set; }

        [Required, MaxLength(5000, ErrorMessage = "Barcode should be less than 5000 charcaters")]
        public string Barcode { get; set; }

        [Required, Range(1, 100000000)]
        public double Price { get; set; }

        private readonly IBookService _bookService;
        public CreateBookModel()
        {
            _bookService = Startup.AutofacContainer.Resolve<IBookService>();
        }

        public CreateBookModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        internal void CreateBook()
        {
            var book = new Book
            {
                Title = Title,
                Barcode = Barcode,
                Price = Price
            };

            _bookService.CreateBook(book);
        }
    }
}
