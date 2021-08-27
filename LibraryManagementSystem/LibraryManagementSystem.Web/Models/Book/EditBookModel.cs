using Autofac;
using LibraryManagementSystem.Reading.BuisnessObjects;
using LibraryManagementSystem.Reading.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.Models
{
    public class EditBookModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Title should be less than 50 charcaters")]
        public string Title { get; set; }

        [Required, MaxLength(5000, ErrorMessage = "Barcode should be less than 5000 charcaters")]
        public string Barcode { get; set; }

        [Required, Range(1, 100000000)]
        public double? Price { get; set; }

        private readonly IBookService _bookService;
        public EditBookModel()
        {
            _bookService = Startup.AutofacContainer.Resolve<IBookService>();
        }

        public EditBookModel(IBookService bookService)
        {
            _bookService = bookService;
        }


        public void LoadModelData(int id)
        {
            var book = _bookService.GetBook(id);
            Title = book.Title;
            Barcode = book.Barcode;
            Price = book?.Price;
        }

        internal void Update()
        {
            var book = new Book
            {
                Id = Id.HasValue ? Id.Value : 0,
                Title = Title,
                Barcode = Barcode,
                Price = Price.HasValue ? Price.Value : 0,
            };
            _bookService.UpdateBook(book);
        }
    }
}
