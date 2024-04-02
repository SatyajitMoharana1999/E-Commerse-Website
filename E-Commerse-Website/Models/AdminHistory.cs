using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class AdminHistory
    {
        [Key]
        public int AH_id { get; set; }
        public DateTime AH_time { get; set; }
        public string? AH_title { get; set; }
        public string? AH_description { get; set; }
        public bool AH_deleted { get; set; }
        public int admin_id { get; set; }
        public Admin? Admin { get; set; }

    }
}
