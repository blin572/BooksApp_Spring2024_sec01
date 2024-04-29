using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024_sec01.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderId { get; set; }
        [ValidateNever]
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int BookID { get; set; }
        [ValidateNever]
        [ForeignKey("BookID")]
        public Books Books { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
