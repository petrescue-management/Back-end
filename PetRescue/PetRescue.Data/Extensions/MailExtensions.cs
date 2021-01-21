using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public class MailArguments
    {
        public string MailTo { get; set; }
        public string Bcc { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string SmtpHost { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string MailFrom { get; set; }
        
    }
    public partial class MailExtensions
    {
        public MailExtensions()
        {

        }
        public static Tuple<bool, string> Send(MailArguments mailArgs, List<Attachment> attachments, bool iSsl, Dictionary<string, string> headers)
        {
            var networkCredential = new NetworkCredential
            {
                Password = mailArgs.Password,
                UserName = mailArgs.MailFrom
            };
            var mailMsg = new MailMessage
            {
                Body = mailArgs.Message,
                Subject = mailArgs.Subject,
                IsBodyHtml = true
            };
            mailMsg.To.Add(mailArgs.MailTo);
            if (ValidationExtensions.IsNotNullOrEmpty(headers))
            {
                foreach (var header in headers)
                {
                    mailMsg.Headers.Add(header.Key, header.Value);
                }
            }
            if (ValidationExtensions.IsNotNullOrEmptyOrWhiteSpace(mailArgs.Bcc))
            {
                string[] bccIds = mailArgs.Bcc.Split(",");
                if (ValidationExtensions.IsNotNullOrEmpty(bccIds))
                {
                    foreach (var bcc in bccIds)
                    {
                        mailMsg.Bcc.Add(bcc);
                    }
                }
            }
            if (ValidationExtensions.IsNotNull(attachments))
            {
                foreach (var attachment in attachments)
                {
                    if (ValidationExtensions.IsNotNull(attachment))
                    {
                        mailMsg.Attachments.Add(attachment);
                    }
                }
            }
            mailMsg.From = new MailAddress(mailArgs.MailFrom, mailArgs.Name);
            var smtpClient = new SmtpClient(mailArgs.SmtpHost)
            {
                Port = Convert.ToInt32(mailArgs.Port),
                EnableSsl = iSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = networkCredential
            };
            smtpClient.Send(mailMsg);
            return Tuple.Create(true, "Successfully");
        }
    }
    
   
}
