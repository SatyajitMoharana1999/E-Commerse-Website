using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerse_Website.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Name Should Be Less Than 50 Characters")]
        public string? product_name { get; set; }
        [Required]
        [Range(1,1000,ErrorMessage = "Price Should Be In Between Rs. 1 to 1000")]
        public string? product_price{ get; set; }
        [Required]
        public string? product_description{ get; set; }
        [ValidateNever]
        public string? product_image{ get; set; }
        public bool product_deleted { get; set; }
        [Required]
        public int cat_id { get; set; }
        public Category Category { get; set; }
        public int adm_id { get; set; }
        public Admin Admin { get; set; }
        [NotMapped]
        public IFormFile? product_img_data { get; set; }
    }
}
