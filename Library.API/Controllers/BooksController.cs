using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            this._bookService = bookService;
        }
        [HttpGet("[action]")]
        public async Task<CustomResponse<List<BookViewModel>>> GetBooks() => await _bookService.GetBooks();
        [HttpGet("[action]")]
        public async Task<CustomResponse<List<BookViewModel>>> GetBookById(int Id) => await _bookService.GetBookById(Id);
        [HttpPost("[action]")]
        public async Task<CustomResponse<List<BookViewModel>>> Create([FromQuery] BookCreateModel createModel) => await _bookService.Create(createModel);
        [HttpPut("[action]")]
        public async Task<CustomResponse<List<BookViewModel>>> Update([FromQuery] BookUpdateModel updateModel) => await _bookService.Update(updateModel);
        [HttpDelete("[action]")]
        public async Task<CustomResponse<BookViewModel>> Delete(int Id) => await _bookService.Delete(Id);
    }
}
