using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksApp_Spring2024_sec01.Models.ViewModels
{
    public class BookWIthCategoriesVM
    {
        public  Books Book { get; set; }

        [ValidateNever]
        public  IEnumerable<SelectListItem> ListOfCategories { get; set; }
    }
}
