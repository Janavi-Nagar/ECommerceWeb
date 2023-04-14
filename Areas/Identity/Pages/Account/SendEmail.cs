using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace ECommerceWeb.Areas.Identity.Pages.Account
{
    public class SendEmail : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("janavi4959@gmail.com");
                mailMessage.Subject = subject;
                mailMessage.Body = email + htmlMessage;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(email));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = "janavi4959@gmail.com";
                NetworkCred.Password = "wclbzzipondcntny";
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;

                await smtp.SendMailAsync(mailMessage);
            } 
            
        }
    }
}
