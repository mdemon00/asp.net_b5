using LibraryManagementSystem.Reading.BuisnessObjects;
using LibraryManagementSystem.Reading.Exceptions;
using LibraryManagementSystem.Reading.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Reading.Services
{
    public class BookService : IBookService
    {
        private readonly IReadingUnitOfWork _readingUnitOfWork;
        public BookService(IReadingUnitOfWork readingUnitOfWork)
        {
            _readingUnitOfWork = readingUnitOfWork;
        }

        public void CreateBook(Book book)
        {
            if (book == null)
                throw new InvalidParameterException("Book was not provided");

            _readingUnitOfWork.Books.Add(new Entites.Book
            {
                Title = book.Title,
                Barcode = book.Barcode,
                Price = book.Price
            });

            _readingUnitOfWork.Save();
        }

        public void DeleteBook(int id)
        {
            _readingUnitOfWork.Books.Remove(id);
            _readingUnitOfWork.Save();
        }

        public Book GetBook(int id)
        {
            var book = _readingUnitOfWork.Books.GetById(id);

            if (book == null) return null;

            return new Book
            {
                Id = book.Id,
                Title = book.Title,
                Barcode = book.Barcode,
                Price = book.Price
            };
        }

        public (IList<Book> records, int total, int totalDisplay) GetBooks(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var bookData = _readingUnitOfWork.Books.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Title.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from book in bookData.data
                              select new Book
                              {
                                  Id = book.Id,
                                  Title = book.Title,
                                  Barcode = book.Barcode,
                                  Price = book.Price
                              }).ToList();
            return (resultData, bookData.total, bookData.totalDisplay);
        }

        public void UpdateBook(Book book)
        {
            if (book == null)
                throw new InvalidOperationException("Book is missing");

            var bookEntity = _readingUnitOfWork.Books.GetById(book.Id);

            if (bookEntity != null)
            {
                bookEntity.Title = book.Title;
                bookEntity.Barcode = book.Barcode;
                bookEntity.Price = book.Price;

                _readingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find book");
        }
    }
}
