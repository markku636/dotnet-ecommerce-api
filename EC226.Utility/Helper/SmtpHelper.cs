using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using EC226.Configuration;
using NPOI.XWPF.UserModel;

namespace EC226.Utility.Helper
{
    public static class SmtpHelper
    {
        private static SmtpClient CreateSMTP(string UserName, string Password, string Host) {
            return new SmtpClient {
                Host = Host,
                Credentials = new NetworkCredential(UserName, Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public static void SendMail(string subject, string receiverEmail, string body, List<string> ccToEmails = null) {
            SendMailBase(subject, receiverEmail, body, AppSettingsConstVars.Mails.AccountEmail, AppSettingsConstVars.Mails.AccountEmailPass, AppSettingsConstVars.Mails.Host, ccToEmails);
        }

        public static void SendMailBase(string subject, string receiverEmail, string body, string accountEmail, string accountEmailPass, string host, List<string> ccToEmails = null) {
            var smtp = CreateSMTP(accountEmail, accountEmailPass, host);

            smtp.EnableSsl = true;
            smtp.Port = 587;

            var msg = new MailMessage {
                From = new MailAddress(accountEmail, accountEmail),
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };

            msg.To.Add(new MailAddress(receiverEmail));
            
            if (ccToEmails != null)
            {
                foreach (var ccTo in ccToEmails)
                {
                    msg.CC.Add(new MailAddress(ccTo));
                }
            }
            

            smtp.Send(msg);

            msg.Dispose();

        }

    }
}
