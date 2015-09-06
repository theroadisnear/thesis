using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;


namespace library_prototype.CustomClass
{
    public class SMTP
    {
        public void SendEmal(string emailReceiver, string pincode)
        {
            using (MailMessage mm = new MailMessage("stvpslibrary@gmail.com", emailReceiver))
            {
                mm.Subject = "WOPAC Account Activation";
                mm.Body = "Hi, here's your pincode "+pincode;
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential("stvpslibrary@gmail.com", "stvpslibraryw0p@c");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = nc;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}