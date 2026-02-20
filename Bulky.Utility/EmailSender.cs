using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			//ici on peut utiliser un service de mail comme SendGrid ou SMTP pour envoyer l'email plutard
			return Task.CompletedTask;
		}
	}
}
