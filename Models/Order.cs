using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024_sec01.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public DateOnly OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        [NotMapped]
        public DateOnly ShippingDate { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public string? SessionID {  get; set; }
        public string? PaymentIntentID { get; set; }
    }
}
