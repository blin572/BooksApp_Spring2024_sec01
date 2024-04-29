namespace BooksApp_Spring2024_sec01.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<Cart> CartItems { get; set; }

        public Order Order { get; set; }

    }
}
