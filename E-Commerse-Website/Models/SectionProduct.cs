using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerse_Website.Models
{
    public class SectionProduct
    {
        [Key]
        public int section_product_id { get; set; }
        public int sort_order { get; set; }
        public bool is_visible { get; set; }
        public bool section_product_deleted { get; set; }

        [ForeignKey("Section")]
        public int section_id { get; set; }
        public Section Section { get; set; }

        [ForeignKey("Product")]
        public int prod_id { get; set; }
        public Product Product { get; set; }
    }
}
