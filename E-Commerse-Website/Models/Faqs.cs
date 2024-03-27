using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
    public class Faqs
    {
        [Key]
        public int faq_id { get; set; }
        public string faq_question{ get; set; }
        public string faq_answer{ get; set; }
    }
}
