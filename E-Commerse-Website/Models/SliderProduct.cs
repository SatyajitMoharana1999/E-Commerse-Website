using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerse_Website.Models
{
    public class SliderProduct
    {
        [Key]
        public int slider_product_id { get; set; }
        public int sort_order { get; set; }
        public bool is_visible { get; set; }
        public bool slider_product_deleted { get; set; }
        public string? product_image { get; set; }

        [ForeignKey("Slider")]
        public int slider_id { get; set; }
        public Slider Slider { get; set; }

        [ForeignKey("Product")]
        public int prod_id { get; set; }
        public Product Product { get; set; }
    }
}
