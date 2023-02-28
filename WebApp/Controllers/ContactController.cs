using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class ContactController : Controller
	{
        private readonly ReCaptcha _captcha;

        public ContactController(ReCaptcha captcha)
        {
            _captcha = captcha;
        }

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Emailsent([FromForm] Email email)
		{
			if (ModelState.IsValid)
			{
				//Captcha
                if (!Request.Form.ContainsKey("g-recaptcha-response")) return View("Contact", this);
                var captcha = Request.Form["g-recaptcha-response"].ToString();
				if (!await _captcha.IsValid(captcha))
				{
					return View("MailNietVerstuurd");
				}
				else
				{
					//Send mail
					await SendEmail(email.Name, email.EmailAddress, email.EmailBody);
					return View("MailVerstuurd");
				}
			}
			return Index();

        }

		public static async Task SendEmail(string name, string emailadress, string plainTextContent_)
		{
			var apiKey = "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";
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("s1169004@student.windesheim.nl", "s1169004@student.windesheim.nl");
			var subject = $"Contact formulier {name}";
			var to = new EmailAddress("s1169004@student.windesheim.nl", "s1169004@student.windesheim.nl");
			var plainTextContent = $"{name}, {emailadress}, {plainTextContent_}";
			var htmlContent = $"{name}, {emailadress}, {plainTextContent_}";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(msg);
		}
	}
}