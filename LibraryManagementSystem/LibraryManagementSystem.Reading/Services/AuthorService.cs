using LibraryManagementSystem.Reading.BuisnessObjects;
using LibraryManagementSystem.Reading.Exceptions;
using LibraryManagementSystem.Reading.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Reading.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IReadingUnitOfWork _readingUnitOfWork;
        public AuthorService(IReadingUnitOfWork readingUnitOfWork)
        {
            _readingUnitOfWork = readingUnitOfWork;
        }

        public void CreateAuthor(Author author)
        {
            if (author == null)
                throw new InvalidParameterException("Author was not provided");

            _readingUnitOfWork.Authors.Add(new Entites.Author
            {
                Name = author.Name,
                DateOfBirth = author.DateOfBirth,
            });

            _readingUnitOfWork.Save();
        }

        public void DeleteAuthor(int id)
        {
            _readingUnitOfWork.Authors.Remove(id);
            _readingUnitOfWork.Save();
        }

        public Author GetAuthor(int id)
        {
            var author = _readingUnitOfWork.Authors.GetById(id);

            if (author == null) return null;

            return new Author
            {
                Id = author.Id,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth
            };
        }

        public (IList<Author> records, int total, int totalDisplay) GetAuthors(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var authorData = _readingUnitOfWork.Authors.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from author in authorData.data
                              select new Author
                              {
                                  Id = author.Id,
                                  Name = author.Name,
                                  DateOfBirth = author.DateOfBirth
                              }).ToList();
            return (resultData, authorData.total, authorData.totalDisplay);
        }

        public void UpdateAuthor(Author author)
        {
            if (author == null)
                throw new InvalidOperationException("Author is missing");

            var authorEntity = _readingUnitOfWork.Authors.GetById(author.Id);

            if (authorEntity != null)
            {
                authorEntity.Name = author.Name;
                authorEntity.DateOfBirth = author.DateOfBirth;

                _readingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find author");
        }
    }
}
