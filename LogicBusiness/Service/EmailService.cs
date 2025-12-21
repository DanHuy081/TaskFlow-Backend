using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    "huydan473@gmail.com",
                    "svgi fcfi rwmr eduj"),
                EnableSsl = true
            };

            var mail = new MailMessage(
                "huydan473@gmail.com",
                to,
                subject,
                body);

            await smtp.SendMailAsync(mail);
        }
    }
}
