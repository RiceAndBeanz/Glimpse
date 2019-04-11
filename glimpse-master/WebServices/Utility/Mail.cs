using System;
using System.Net;
using System.Net.Mail;
using WebServices.Models;

namespace WebServices.Utilities
{
    static class Mail
    {

        //This method is used to create the email template to send the vendor after they signup
        //The name from the sign up form
        //The formatted mail body to inform and congradulate the vendor for signing up
        static string CreateMailBodyForVendor(string companyName)
        {
            string mailBody = "<div> Hello " + companyName + ", <br/>" +
                "Your account is created successfully! Pending verfication for your account. <br/><br/> Regards,<br/> The Glimpse Team</div>";
            return mailBody;
        }

        static string CreateMailSubjectForVendor(string companyName)
        {
            string mailBody = "Glimpse account for " + companyName;
            return mailBody;
        }


        //The method sends mail. Pass the parameters carefully when you call.
        // mailSub = Subject of the email.
        // mailbody = Mailbody of the email.
        // mailTo = The email address to send.

        static public bool SendEmail(Vendor mailTo)
        {
            var fromAddress = new MailAddress("vendor.smtptest@gmail.com", "Glimpse");
            var toAddress = new MailAddress(mailTo.Email, mailTo.CompanyName);
            const string fromPassword = "thisisthetestingpassword";
            string subject = CreateMailSubjectForVendor(mailTo.CompanyName);
            string body = CreateMailBodyForVendor(mailTo.CompanyName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
            {
                smtp.Send(message);
                return true;
            }

        }

    }
}
