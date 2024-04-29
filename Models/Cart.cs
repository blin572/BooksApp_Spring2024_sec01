using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024_sec01.Models
{
    public class Cart
    {
        public int CartId{ get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("BookID")]
        [ValidateNever]
        public Books Books { get; set; }
        [NotMapped]
        public decimal SubTotal { get; set; }
    }

    
}
