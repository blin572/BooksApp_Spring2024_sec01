using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BooksApp_Spring2024_sec01.Models
{
    public class Books
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string? ImgUrl { get; set; }

        [ForeignKey("CategoryID")]
        public Category? category { get; set; } //navigational property
    }
}
