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
        public const string APPROVE_REGISTRATION_FORM = "THƯ THÔNG BÁO TRỞ THÀNH TRUNG TÂM CỨU HỘ CỦA HỆ THỐNG";
        public const string REJECT_REGISTRATION_FORM = "THƯ THÔNG BÁO TỪ CHỐI TRỞ THÀNH TRUNG TÂM CỦA HỆ THỐNG";
        public const string APPROVE_REGISTRATION_VOLUNTEER = "THƯ THÔNG BÁO TRỞ THÀNH TÌNH NGUYỆN VIÊN";
        public const string REJECT_REGISTRATION_VOLUNTEER = "THƯ THÔNG BÁO TỪ CHỐI TRỞ THÀNH TÌNH NGUYỆN VIÊN";
        public const string ORG_NAME = "RescueMe";
        public const string APP_VOLUNTEER_NAME = "VolunteerApp";
        public const string WEBSITE_NAME = "petrescue.com";
        public static string ApproveRegistrationCenter(string mailTo, CenterRegistrationFormViewModel model)
        {
            return "<!DOCTYPE html><html><head><title></title></head><body data-new-gr-c-s-loaded = '14.1002.0' spellcheck = 'false'>< p style = 'text-align: left;'><strong><span style = 'font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p>Chào bạn, " +
                model.Email +
                "</p><p> Chúng tôi là đại diện đến từ hệ thống cứu hộ<strong> " +
                ORG_NAME +
                "</strong></p><p> Dựa vào các thông tin mà <strong> " +
                model.Email +
                " </strong> đã cung cấp, chúng tôi đã tra cứu, xác minh và phê duyệt trung tâm của bạn( <strong> " +
                model.CenterName +
                " </strong> ) đủ điều kiện để trở thành một trung tâm thành viên của hệ thống cứu hộ " +
                ORG_NAME +
                ".</p><p> Để sử dụng hệ thống của chúng tôi, bạn cần phải dùng email đã đăng ký; ( <span style = 'color: rgb(44, 130, 201);'><a href = 'mailto:" +
                model.Email +
                "'> " +
                model.Email +
                " </a> &nbsp;</span>) đăng nhập vào website: <a href = '" +
                WEBSITE_NAME +
                "' > " +
                WEBSITE_NAME +
                " </a>.</p><p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi.</p><p> Thân ái </p>" +
                "<p> --------------------------------------------------</p>" +
                "<p style = 'line-height: 0.1;'><sub> Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p style = 'line-height: 0.2;'><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +
                " </sub></p></body></html>";
        }
        public static string RejectRegistrationCenter(string mailTo, CenterRegistrationFormViewModel model, string error)
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
            return "<!DOCTYPE html><html><head><title></title></head>" +
                "<body data-new-gr-c-s-loaded='14.1002.0' spellcheck='false'><p style = 'text-align: left;'><strong><span style ='font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p> Chào bạn, " +
                mailto +
                " </p><p> Chúng tôi đến từ hệ thống cứu hộ <strong> " +
                ORG_NAME +
                "</strong></p><p> Chúc mừng bạn, bạn đã trở thành một tình nguyện viên của " +
                model.CenterName +
                "</p><p> Bạn sẽ phải cần đăng nhập tại ứng dụng<span style= 'color: rgb(44, 130, 201);'> " +
                APP_VOLUNTEER_NAME +
                "</span></p><p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi </p>" +
                "<p> Thân ái </p>" +
                "<p> --------------------------------------------------</p>" +
                "<p style  = 'line-height: 1;'><span style = 'font-size: 14px;'>" +
                " Mọi thông tin chi tiết xin liên hệ :</span></p>" +
                "<p style = 'line-height: 0.5;' ><span style = 'font-size: 14px;'> " +
                model.CenterName +
                "</span></p><p style = 'line-height: 0.5;'><span style = 'font-size: 14px;'> SDT: " +
                model.Phone +
                " </span></p><p style = 'line-height: 0.5;'><span style = 'font-size: 14px;'> Địa chỉ: " +
                model.Address +
                 " </span></p><p style = 'line-height: 0.5;'><span style = 'font-size: 14px;'> Email: " +
                model.Email +
                "</span></p><p> --------------------------------------------------</p><p>" +
                "<p style='line-height: 0.1;'><sub>Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p style = 'line-height: 0.2;'><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +" </sub></p></body></html>";
        }
        public static string RejectRegistrationVolunteer(string mailTo,string error, CenterViewModel model)
        {
            return "<!DOCTYPE html><html><head><title></title></head><body data-new-gr-c-s-loaded = '14.1002.0' spellcheck = 'false'><p style = 'text-align: left;' ><strong><span style='font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p>Chào bạn, " +
                mailTo +
                "</p><p> Chúng tôi đến từ hệ thống cứu hộ<strong>" +
                ORG_NAME +
                "</strong></p><p> Cảm ơn bạn đã đăng ký để trở thành tình nguyện viện của " +
                model.CenterName +
                ". Nhưng chúng tối rất tiếc phải thông báo rằng, trung tâm " +
                model.CenterName +
                " đã từ chối đề nghị trở thành tình nguyện viên với lý do như sau:</p><ul>" +
                error +
                "</ul><p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi.</p><p> Thân ái </p>" +
                "<p> --------------------------------------------------</p>" +
                "<p style = 'line-height: 1;'><span style = 'font-size: 14px;'> Mọi thông tin chi tiết xin liên hệ :</span>" +
                "</p><p style = 'line-height: 0.5;' ><span style = 'font-size: 14px;'> " +
                model.CenterName +
                "</span></p><p style = 'line-height: 0.5;'><span style = 'font-size: 14px;'> SĐT: " +
                model.Phone +
                "</span></p>" +
                "<p style = 'line-height: 0.5;'><span style = 'font-size: 14px;'> Địa chỉ: " +
                model.Address +
                "<p style = 'line-height: 0.5;' >< span style = 'font-size: 14px;'> Email: " +
                model.Email +
                "</span></p><p> --------------------------------------------------</p>" +
                "<p style = 'line-height: 0.1;' >< sub > Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p style = 'line-height: 0.2;'><sub> Góp ý cho hệ thống qua mail: " +
                MAIL + 
                "</body></html> ";
        }
    }
}
