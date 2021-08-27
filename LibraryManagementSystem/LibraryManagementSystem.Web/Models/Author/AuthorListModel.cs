using Autofac;
using LibraryManagementSystem.Reading.Services;
using LibraryManagementSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Web.Models
{
    public class AuthorListModel
    {
        private readonly IAuthorService _authorService;
        public AuthorListModel()
        {
            _authorService = Startup.AutofacContainer.Resolve<IAuthorService>();
        }

        public AuthorListModel(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        internal object GetAuthors(DataTablesAjaxRequestModel tableModel)
        {
            var data = _authorService.GetAuthors(tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name", "DateOfBirth" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name,
                                record.DateOfBirth.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _authorService.DeleteAuthor(id);
        }
    }
}
