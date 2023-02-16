using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult MailVerstuurd()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Emailsent([FromForm] Email email)
		{
			if (ModelState.IsValid)
			{
				await SendEmail(email.Name, email.EmailSubject, email.EmailAddress, email.EmailBody);
				return View("MailVerstuurd");
			}
			else
			{
				return View("MailNietVerstuurd");
			}
		}

		public static async Task SendEmail(string name, string subject, string emailadress, string plainTextContent)
		{
			var apiKey = "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";
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress(emailadress, name);
			var to = new EmailAddress("s1169004@student.windesheim.nl", "Mike");
			var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(msg);
		}
	}
}