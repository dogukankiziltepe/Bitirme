using Bitirme.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Interfaces
{
    public interface IBookService
    {
        public bool AddBook(BookViewModel model);
        public List<BookViewModel> GetBooks();
    }
}
