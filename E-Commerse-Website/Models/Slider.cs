using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Slider
    {
        [Key]
        public int slider_id { get; set; }
        public string? slider_name { get; set; }
        public string? slider_description { get; set; }

        public bool is_active { get; set; }
        public bool slider_deleted { get; set; }

        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }

        public List<SliderProduct> SliderProducts { get; set; }    

    }
}
