using Library.API.Database.Context;
using Library.API.Database.Entities;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class BookService : IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly LibraryContext _db;
        private readonly IConfiguration _configuration;
        public BookService(ILogger<BookService> logger, LibraryContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        public async Task<CustomResponse<List<BookViewModel>>> GetBooks()
        {
            try
            {
                var Books = await _db.Books.ToListAsync();
                if (Books.Count == 0)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"

                    }; //არის თუ არა დატაბაზა ცარიელი/ არ მოიძებნება წიგნებზე ინფორმაცია
                }
                var bookView = new List<BookViewModel>();
                foreach (var book in Books)
                {
                    bookView.Add(new BookViewModel()
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        PublicationYear = book.PublicationYear,
                    });
                    // ლისტში წიგნების დამატება
                }
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 200,
                    Result = bookView,
                };
                //წიგნებზე ინფორმაციის გამოტანა

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 404,
                    ErrorMessage = "Something went wrong"

                };

            };

        }
        public async Task<CustomResponse<List<BookViewModel>>> GetBookById(int Id)
        {
            int i = 0;//დროული შემოწმება. ამოწმებს არსებობს თუ არა წიგნი მოცემული Id-თ
            try
            {

                var Books = await _db.Books.ToListAsync();
                if (Books.Count == 0)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"

                    };
                }
                if (Id <= 0)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 422,
                        ErrorMessage = "Invalid Id, please type correct information in."
                    };
                }
                var bookView = new List<BookViewModel>();
                foreach (var book in Books)
                {
                    if (book.Id == Id)
                    {
                        i++;//i ერთის ტოლი უნდა იყოს თუ წიგნი მოიძებნა
                        bookView.Add(new BookViewModel()
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            PublicationYear = book.PublicationYear,
                        });

                    }
                }
                if (i <= 0 || i != 1)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Entity does not exist"
                    };
                }
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 200,
                    Result = bookView,
                };
                
            }
            
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 404,
                    ErrorMessage = "Something went wrong"

                };
}
}
        public async Task<CustomResponse<List<BookViewModel>>> Create(BookCreateModel createModel)
        {
            try
            {
                var Books = await _db.Books.ToListAsync();
                if (createModel == null)//ცარიელი მონაცემები
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 422,
                        ErrorMessage = "Invalid info",
                    };
                }
                foreach(var books in Books)
                {
                    if (createModel.Title == books.Title && createModel.Author == books.Author && createModel.PublicationYear == books.PublicationYear)
                    {
                        return new CustomResponse<List<BookViewModel>>()
                        {
                            StatusCode = 422,
                            ErrorMessage = "This book is alrady registered",
                        };//არსებობს თუ არა ასეთი წიგნი დატაბაზაში
                    }
                }
                var book = new Book()
                {
                    Title = createModel.Title,
                    Author = createModel.Author,
                    PublicationYear = createModel.PublicationYear,
                   
                };

                await _db.Books.AddAsync(book);
                await _db.SaveChangesAsync();
                //დამატება 
                var bookView = new List<BookViewModel>();
                bookView.Add(new BookViewModel()
                {
                    Title = book.Title,
                    Author = book.Author,
                    PublicationYear = book.PublicationYear,
                });
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 200,
                    Result = bookView
                };
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            };
        }
        public async Task<CustomResponse<List<BookViewModel>>> Update(BookUpdateModel updateModel)
        {
            int c = 0;
            try
            {
                var Books = await _db.Books.ToListAsync();
                if(updateModel.Id <= 0)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 422,
                        ErrorMessage = "Invalid Id",
                    };
                }
                if (updateModel == null)//ცარიელი მონაცემები
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 422,
                        ErrorMessage = "Invalid info",
                    };
                }
                if (await _db.Books.FirstOrDefaultAsync(x => x.Id == updateModel.Id) == null)
                {
                    return new CustomResponse<List<BookViewModel>>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found."
                    };
                }
                foreach (var books in Books)
                {
                    if (updateModel.Title == books.Title && updateModel.Author == books.Author && updateModel.PublicationYear == books.PublicationYear)
                    {
                        return new CustomResponse<List<BookViewModel>>()
                        {
                            StatusCode = 422,
                            ErrorMessage = "This book is alrady registered",
                        };//არსებობს თუ არა ასეთი წიგნი დატაბაზაში
                    }
                }
                var book = new Book()
                {
                    Title = updateModel.Title,
                    Author = updateModel.Author,
                    PublicationYear = updateModel.PublicationYear,

                };

                 _db.Books.Update(book);
                await _db.SaveChangesAsync();
                // განახლება 
                var bookView = new List<BookViewModel>();
                bookView.Add(new BookViewModel()
                {
                    Title = book.Title,
                    Author = book.Author,
                    PublicationYear = book.PublicationYear,
                });
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 200,
                    Result = bookView
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return new CustomResponse<List<BookViewModel>>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            };
        }

        public async Task<CustomResponse<BookViewModel>> Delete(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return new CustomResponse<BookViewModel>()
                    {
                        StatusCode = 422,
                        ErrorMessage = "Invalid Id"
                    };
                }
                var book = await _db.Books.FirstOrDefaultAsync(x => x.Id == Id);
                if (book == null)
                {
                    return new CustomResponse<BookViewModel>
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"
                    };
                }
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();

                return new CustomResponse<BookViewModel>
                {
                    StatusCode = 200,
                    ErrorMessage = "Deleted Successfully",
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return new CustomResponse<BookViewModel>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            }

        }

    }
}
