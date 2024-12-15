using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Section
    {
        [Key]
        public int section_id { get; set; }
        public string? section_name { get; set; }
        public string? section_description { get; set; }
        public int sort_order { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public bool is_active { get; set; }
        public bool section_deleted { get; set; }
        public List<SectionProduct> SectionProducts { get; set; }
    }
}
