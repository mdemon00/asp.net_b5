using Autofac;
using LibraryManagementSystem.Reading.BuisnessObjects;
using LibraryManagementSystem.Reading.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.Models
{
    public class EditAuthorModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        private readonly IAuthorService _authorService;
        public EditAuthorModel()
        {
            _authorService = Startup.AutofacContainer.Resolve<IAuthorService>();
        }

        public EditAuthorModel(IAuthorService authorService)
        {
            _authorService = authorService;
        }


        public void LoadModelData(int id)
        {
            var author = _authorService.GetAuthor(id);
            Name = author.Name;
            DateOfBirth = author.DateOfBirth;
        }

        internal void Update()
        {
            var author = new Author
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name,
                DateOfBirth = DateOfBirth
            };
            _authorService.UpdateAuthor(author);
        }
    }
}
