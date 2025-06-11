using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Bitirme.BLL.Services
{
    public class EmailService
    {

        public static bool SendEmail(string to, string subject,string body)
        {
            bool gonderimSonucu = false;
            int mailAdresiSayisi = Regex.Matches(to, ";").Count + 1;
            SmtpClient client = new SmtpClient("smtp.yandex.com.tr", 587);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("learnTurkish1@yandex.com", "Learn Turkish"); //gönderici olarak görünen mail bilgileri
            mail.Priority = MailPriority.Normal;
            mail.Subject = subject;
            mail.To.Add(new MailAddress(to, ""));

            mail.Body = body;
            mail.IsBodyHtml = true;

            NetworkCredential girisIzni = new NetworkCredential("learnTurkish1@yandex.com", "jetvlzjlbatdswsc");
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = girisIzni;
            try
            {
                client.Send(mail);
                gonderimSonucu = true;
                return gonderimSonucu;
            }
            catch (Exception ex)
            {
                gonderimSonucu = false;
                return gonderimSonucu;
            }
        }
    }
}