using Autofac;
using LibraryManagementSystem.Reading.Services;
using LibraryManagementSystem.Reading.BuisnessObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.Models
{
    public class CreateAuthorModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        private readonly IAuthorService _authorService;
        public CreateAuthorModel()
        {
            _authorService = Startup.AutofacContainer.Resolve<IAuthorService>();
        }

        public CreateAuthorModel(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        internal void CreateAuthor()
        {
            var author = new Author
            {
                Name = Name,
                DateOfBirth = DateOfBirth
            };

            _authorService.CreateAuthor(author);
        }
    }
}
