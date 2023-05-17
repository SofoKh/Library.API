using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    public class BookCreateModel
    {
        [Required, MinLength(2) ]
        public string Title { get; set; }

        [Required,MinLength(2)]
        public string Author { get; set; }

        [Required, MinLength(4)]
        public int PublicationYear { get; set; }
    }
}
