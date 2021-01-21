using PetRescue.Data.ConstantHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Extensions
{

    public partial class MailFormat
    {
       public static MailArguments MailApproveRegistrationCenter(string mailTo)
        {
            var mailArguments = new MailArguments
            {
                MailFrom = MailConstant.MAIL,
                Password = MailConstant.PASSWORD,
                MailTo = mailTo,
                Subject = "PetRescue Approve the Registration Center",
                Port = MailConstant.PORT,
                SmtpHost = MailConstant.HOST,
                Name = MailConstant.NAME,
                Message = MailConstant.ApproveRegistrationCenter(mailTo)
                
            };
            return mailArguments;
        }
    }
}
