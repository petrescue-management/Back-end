using PetRescue.Data.ConstantHelper;
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
        public static bool Send(MailArguments mailArgs, List<Attachment> attachments, bool iSsl, Dictionary<string, string> headers)
        {
            bool check = false;
            try
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
                check = true;
                return check;
            }
            catch
            {
                return check;
            }
            
        }
        public static bool SendBySendGrid(MailArguments mailArgs, List<Attachment> attachments, Dictionary<string, string> headers)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.Subject = mailArgs.Subject;
                message.To.Add(mailArgs.MailTo);
                message.Body = mailArgs.Message;
                message.From = new MailAddress(mailArgs.MailFrom, "Rescue Me");
                if (ValidationExtensions.IsNotNullOrEmpty(headers))
                {
                    foreach (var header in headers)
                    {
                        message.Headers.Add(header.Key, header.Value);
                    }
                }
                if (ValidationExtensions.IsNotNullOrEmptyOrWhiteSpace(mailArgs.Bcc))
                {
                    string[] bccIds = mailArgs.Bcc.Split(",");
                    if (ValidationExtensions.IsNotNullOrEmpty(bccIds))
                    {
                        foreach (var bcc in bccIds)
                        {
                            message.Bcc.Add(bcc);
                        }
                    }
                }
                if (ValidationExtensions.IsNotNull(attachments))
                {
                    foreach (var attachment in attachments)
                    {
                        if (ValidationExtensions.IsNotNull(attachment))
                        {
                            message.Attachments.Add(attachment);
                        }
                    }
                }
                SmtpClient client = new SmtpClient(mailArgs.SmtpHost) 
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("apikey", mailArgs.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = mailArgs.Port,
                    Timeout = 99999,
                    EnableSsl = false
                };
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    
   
}
