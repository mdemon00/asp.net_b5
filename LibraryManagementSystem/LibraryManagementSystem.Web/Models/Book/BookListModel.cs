using Autofac;
using LibraryManagementSystem.Reading.Services;
using LibraryManagementSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Web.Models
{
    public class BookListModel
    {
        private readonly IBookService _bookService;
        public BookListModel()
        {
            _bookService = Startup.AutofacContainer.Resolve<IBookService>();
        }

        public BookListModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        internal object GetBooks(DataTablesAjaxRequestModel tableModel)
        {
            var data = _bookService.GetBooks(tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Title", "Barcode", "Price" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Title,
                                record.Barcode,
                                record.Price.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _bookService.DeleteBook(id);
        }
    }
}
