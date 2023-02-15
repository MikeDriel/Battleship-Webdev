using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;


namespace WebApp.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}


		public static async Task SendEmail()
		{
			var apiKey = "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";
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("s1169004@student.windesheim.nl", "Example User");
			var subject = "Sending with SendGrid is Fun";
			var to = new EmailAddress("s1169004@student.windesheim.nl", "Example User");
			var plainTextContent = "and easy to do anywhere, even with C#";
			var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(msg);
		}
	}
}