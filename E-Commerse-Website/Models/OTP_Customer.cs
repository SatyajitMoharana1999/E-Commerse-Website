using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.Models
{
	public class OTP_Customer
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("Customer")]
		public int CustomerId { get; set; }
		public string Code { get; set; }
		public DateTime CreatedAt { get; set; }
		public virtual Customer Customer { get; set; }
	}
}
