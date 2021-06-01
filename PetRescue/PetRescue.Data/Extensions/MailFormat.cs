using PetRescue.Data.ConstantHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Extensions
{

    public partial class MailFormat
    {
        public static MailArguments MailModel (string mailTo, string formatMail, string subject)
       {
            var mailArguments = new MailArguments
            {
                MailFrom = MailConstant.MAIL,
                Password = MailConstant.PASSWORD1 + MailConstant.PASSWORD2 + MailConstant.PASSWORD3,
                MailTo = mailTo,
                Subject = subject,
                Port = MailConstant.PORT,
                SmtpHost = MailConstant.HOST,
                Name = MailConstant.NAME,
                Message = formatMail
                
            };
            return mailArguments;
       }
    }
}
