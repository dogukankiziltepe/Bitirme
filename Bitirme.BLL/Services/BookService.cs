using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Medias;
using Bitirme.DAL.Entities.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;   
        }
        public bool AddBook(BookViewModel model)
        {
            _bookRepository.Add(new Book
            {
                CoverName = model.CoverName,
                FileName = model.FileName,
                CreatedDate = DateTime.UtcNow,
                Title = model.Title,
            });
            _bookRepository.SaveChanges();
            return true;
        }

        public List<BookViewModel> GetBooks()
        {
            return _bookRepository.GetAll().Select(x => new BookViewModel
            {
                CoverName = x.CoverName,
                FileName = x.FileName,
                Id = x.Id,
                Title = x.Title,
            }).ToList();
        }
    }
}
