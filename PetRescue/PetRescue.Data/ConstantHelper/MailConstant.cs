using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ConstantHelper
{
    public class MailConstant
    {
        public const string MAIL = "petrescue2021@gmail.com";
        public const string PASSWORD = "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";
        public const int PORT = 587;
        public const string HOST = "smtp.sendgrid.net";
        public const string NAME = "PetResuce";
        public const string APPROVE_REGISTRATION_FORM = "Pet Rescue approve your registration form";
        public const string REJECT_REGISTRATION_FORM = "Pet Rescue reject your registration form";
        public const string APPROVE_REGISTRATION_VOLUNTEER = "Pet Rescue approve your volunteer form";
        public const string REJECT_REGISTRATION_VOLUNTEER = "Pet Rescue reject your volunteer form";
        public const string ORG_NAME = "RescueMe";
        public const string APP_VOLUNTEER_NAME = "VolunteerApp";
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
        public static string RejectRegistrationCenter(string mailTo)
        {
            return "<html>" +
                "<head>" +
                    "<title>PetRescue</title>" +
                        "<link href='https://svc.webspellchecker.net/spellcheck31/lf/scayt3/ckscayt/css/wsc.css' rel='stylesheet' type='text/css' />" +
                "</head>" +
                                "<body>Hi," +
                                mailTo +
                                "<br /><br />" +
                                    "Sorry!!!!!! "+
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

        public static string ApproveRegistrationVolunteer(string mailto, CenterViewModel model)
        {
            return "Thư thông báo từ hệ thống cứu hộ chó mèo &emsp; <br/>      &emsp; <br/> &emsp; <br/>      &emsp; <br/>    &emsp; <br/>    &emsp; <br/>      "
                + ORG_NAME +
            "&emsp; <br/> Hi, "
            + mailto +
            "&emsp; <br/> Chúng tôi đến từ hệ thông cứu hộ chó mèo "
            + ORG_NAME +
            "&emsp; <br/> Chúc mừng bạn, bạn đã trở thành một tình nguyện viên của trung tâm "
            + model.CenterName +
            "&emsp; <br/> Bạn sẽ phải cần đăng nhập tại ứng dụng "
            + APP_VOLUNTEER_NAME +
            "&emsp; <br/> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi &emsp; <br/> Thân ái&emsp; <br/> Mọi thông tin chi tiết xin liên hệ:&emsp; <br/>"
            + model.CenterName +
            ", SDT: "
            + model.Phone +
            " , địa chỉ : "
            + model.Address +
            "&emsp; <br/> Góp ý cho hệ thống qua mail: "
            + MAIL +
            ";";
        }
        public static string RejectRegistrationVolunteer(string mailTo,ReturnVolunteerError error, CenterViewModel model)
        {
            return "";
        }
    }
    public class ReturnVolunteerError
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AnotherReason { get; set; }
    }
    
}
