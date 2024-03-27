using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }
        public string? category_name { get; set; }
        public bool category_deleted { get; set; }
        public List<Product>? Product { get; set; }
        public int ProductCount => Product?.Count ?? 0;
    }
}
