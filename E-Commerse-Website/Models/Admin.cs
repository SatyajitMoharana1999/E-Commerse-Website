using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Admin
    {
        [Key]
        public int admin_id { get; set; }
        [Required]
        public string? admin_name { get; set; }
        [Required]
        public string? admin_email { get; set; }
        public string? admin_password { get; set; }
        [ValidateNever]
        public string? admin_image { get; set; }

        public List<Product>? Product { get; set; }
        public int ProductCount => Product?.Count ?? 0;
        public List<AdminHistory>? History { get; set; }
    }
}
