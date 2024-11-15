using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerse_Website.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }

        [ForeignKey("prod")]
        public int prod_id { get; set; }
        public Product prod { get; set; }
        public int cust_id { get; set; }
        public int product_quantity { get; set; }
        public int cart_status { get; set; } = 0;
    }
}
