using Library.API.Models;

namespace Library.API.Interfaces
{
    public interface IBookService
    {
        Task<CustomResponse<List<BookViewModel>>> GetBooks();
        Task<CustomResponse<List<BookViewModel>>> GetBookById(int Id);
        Task<CustomResponse<List<BookViewModel>>> Create(BookCreateModel createModel);
        Task<CustomResponse<List<BookViewModel>>> Update(BookUpdateModel updateModel);
        Task<CustomResponse<BookViewModel>> Delete(int Id);
    }
}
