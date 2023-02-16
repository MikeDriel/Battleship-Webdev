using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class Email
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string EmailAddress { get; set; }
		[Required]
		[StringLength(600, ErrorMessage = "Email length can't be more than 600.")]
		public string EmailBody { get; set; }
		[Required]
		[StringLength(200, ErrorMessage = "Subject length can't be more than 200.")]
		public string EmailSubject { get; set; }

	}
}