using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Feedback
    {
        [Key]
        public int feedback_id { get; set; }
        public string feedback_name { get; set; }
        public string feedback_message{ get; set; }
    }
}
