using E_Commerse_Website.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerse_Website.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
        public IEnumerable<SelectListItem> categoryList;
    }
}
