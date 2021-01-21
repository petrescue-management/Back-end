using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ConstantHelper
{
    public class MailConstant
    {
        public const string MAIL = "petrescue2021@gmail.com";
        public const string PASSWORD = "pet012345";
        public const int PORT = 587;
        public const string HOST = "smtp.gmail.com";
        public const string NAME = "PetResuce";

        public static string ApproveRegistrationCenter(string mailTo)
        {
            return "<html>" +
                "<head>" +
                    "<title>PetRescue</title>" +
                        "<link href='https://svc.webspellchecker.net/spellcheck31/lf/scayt3/ckscayt/css/wsc.css' rel='stylesheet' type='text/css' />" +
                "</head>" +
                                "<body>Hi," +
                                mailTo +
                                "<br /><br />" +
                                    "Welcome! Your RescueThem account is now ready. You can now sign in to the&nbsp;RescueThem&nbsp;staff website.<br />" +
                                    "Thank you for using our service. We wish you all the best." +
                                        "<div><br />Regards,</div>" +
                                            "======================<br />The RecueThem Team" +
                                                "<div>" +
                                                    "<div>" +
                                                        "<span style='font-size:14px'><strong>Email:&nbsp;</strong>" +
                                                        MailConstant.MAIL +
                                                        "</span>" +
                                                    "</div>" +
                                                "</div>" +
                                "</body>" +
                "</html>";
        }
    }
    
}
