using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksApp_Spring2024_sec01.Models
{
    public class Category
    {

        public int CategoryID { get; set; }

        [DisplayName("Category Name"), 
            Required(ErrorMessage = "Name of the Category must be provided")]
        public string Name { get; set; }

        [DisplayName("Category Description"), 
            Required(ErrorMessage = "Description of the Category must be provided"), 
            MaxLength(30, ErrorMessage = "The description length cannot be more than 30 characters")]
        public string Description { get; set; }
    }
}
