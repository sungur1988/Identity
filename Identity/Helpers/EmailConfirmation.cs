using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string link, string email)

        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpClient = new SmtpClient
            {


                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("sungur1988@gmail.com", "......."),
                Timeout = 20000
            };

            mail.From = new MailAddress("sungur1988@gmail.com");
            mail.To.Add(email);

            mail.Subject = $"www.bıdıbı.com::Email Doğrulama";
            mail.Body = "<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>email doğrulama linki</a>";
            mail.IsBodyHtml = true;


            smtpClient.Send(mail);
        }
    }
}
