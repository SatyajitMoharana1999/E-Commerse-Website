using System.ComponentModel.DataAnnotations;

namespace E_Commerse_Website.ViewModels
{
	public class CustomerViewModel
	{
		[Required]
		public string customer_name { get; set; }
		[Required, EmailAddress]
		public string customer_email { get; set; }
		[Required, DataType(DataType.Password)]
		public string customer_password { get; set; }
		public string customer_gender { get; set; }
		public string customer_country { get; set; }
		public string customer_city { get; set; }
		public string customer_address { get; set; }
	}
}
