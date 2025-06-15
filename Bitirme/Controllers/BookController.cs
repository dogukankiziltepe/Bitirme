using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitirme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost("CreateBook")]
        public IActionResult CreateBook(BookViewModel model)
        {
            var result = _bookService.AddBook(model);
            return Ok(result);
        }

        [HttpGet("GetBooks")]
        public IActionResult GetBooks()
        {
            var result = _bookService.GetBooks();
            return Ok(result);
        }
    }
}
