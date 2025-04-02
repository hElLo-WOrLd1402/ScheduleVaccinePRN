using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nguyenngoctuananh3001@gmail.com", "dxic tzec adxi mkwq")
            };

            return client.SendMailAsync(
                new MailMessage(from: "nguyenngoctuananh3001@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
