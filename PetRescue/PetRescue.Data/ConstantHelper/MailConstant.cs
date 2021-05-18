using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ConstantHelper
{
    public class MailConstant
    {
        public const string MAIL = "petrescue2021@gmail.com";
        public const string PASSWORD1 = "SG.";
        public const string PASSWORD2 = "973ytHTvRKKEM1SnTFitvw";
        public const string PASSWORD3 = ".P2a2PHuZ8WWNWn5fdAHuGzWMfo9NoQpzy3gPzc3PW3w";
        public const int PORT = 587;
        public const string HOST = "smtp.sendgrid.net";
        public const string NAME = "PetResuce";
        public const string APPROVE_REGISTRATION_FORM = "THƯ THÔNG BÁO TRỞ THÀNH TRUNG TÂM CỨU HỘ CỦA HỆ THỐNG";
        public const string REJECT_REGISTRATION_FORM = "THƯ THÔNG BÁO TỪ CHỐI TRỞ THÀNH TRUNG TÂM CỦA HỆ THỐNG";
        public const string APPROVE_REGISTRATION_VOLUNTEER = "THƯ THÔNG BÁO TRỞ THÀNH TÌNH NGUYỆN VIÊN";
        public const string REJECT_REGISTRATION_VOLUNTEER = "THƯ THÔNG BÁO TỪ CHỐI TRỞ THÀNH TÌNH NGUYỆN VIÊN";
        public const string APPROVE_ADOPTION = "THƯ THÔNG BÁO ĐÃ ĐƯỢC DUYỆT ĐƠN ĐĂNG KÝ NHẬN NUÔI";
        public const string ORG_NAME = "RescueMe";
        public const string APP_VOLUNTEER_NAME = "RescueMeVolunteer";
        public const string WEBSITE_NAME = "https://pet-rescue-system.netlify.app/";
        public const string TIME_WORK = "Từ 8h đến 20h các ngày trong tuần.";
        public const string DEFAULT_DATE = "7 ngày";
        public static string ApproveRegistrationCenter(CenterRegistrationFormViewModel model)
        {
            return "<!DOCTYPE html><html><head><title></title></head><body data-new-gr-c-s-loaded = '14.1002.0' spellcheck = 'false'><p style = 'text-align: left;'><strong><span style = 'font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p>Xin Chào, " +
                model.CenterName +
                "</p><p> Chúng tôi là đại diện đến từ hệ thống cứu hộ<strong> " +
                ORG_NAME +
                "</strong></p><p> Dựa vào các thông tin mà bạn  " +
                " đã cung cấp, chúng tôi đã tra cứu, xác minh và phê duyệt trung tâm của bạn " +
                "đủ điều kiện để trở thành một trung tâm thành viên của hệ thống cứu hộ " +
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
                "<p><sub> Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +
                " </sub></p></body></html>";
        }
        public static string RejectRegistrationCenter(CenterRegistrationFormViewModel model, string error)
        {
            return "<!DOCTYPE html><html><head><title></title></head><body data-new-gr-c-s-loaded = '14.1002.0' spellcheck = 'false'><p style = 'text-align: left;'><strong><span style = 'font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p>Chào bạn, " +
                model.CenterName +
                "</p><p> Chúng tôi là đại diện đến từ hệ thống cứu hộ<strong> " +
                ORG_NAME +
                "</strong></p><p> Dựa vào các thông tin mà bạn " +
                "đã cung cấp, chúng tôi đã tra cứu, xác minh trung tâm của bạn" +
                " và đưa ra quyết định từ chối đơn đăng ký của bạn vì chưa đủ điều kiện để trở thành một trung tâm thành viên của hệ thống cứu hộ " +
                ORG_NAME +
                " với lý do như sau: </p>" +
                "<ul>" +
                error +
                "</ul>" +
                "<p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi.</p><p> Thân ái </p>" +
                "<p> --------------------------------------------------</p>" +
                "<p><sub> Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +
                " </sub></p></body></html>";
        }
        public static string ApproveRegistrationVolunteer(VolunteerRegistrationFormViewModel volunteerModel)
        {
            return "<!DOCTYPE html><html><head><title></title></head>" +
                "<body data-new-gr-c-s-loaded='14.1002.0' spellcheck='false'><p style = 'text-align: left;'><strong><span style ='font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p> Chào bạn, " +
                volunteerModel.LastName + " " + volunteerModel.FirstName +
                " </p><p> Chúng tôi đến từ hệ thống cứu hộ <strong> " +
                ORG_NAME +
                "</strong></p><p> Chúc mừng bạn, bạn đã trở thành một tình nguyện viên của hệ thống<strong>" +
                
                "</strong></p><p> Bạn sẽ phải cần đăng nhập tại ứng dụng<span style= 'color: rgb(44, 130, 201);'> " +
                APP_VOLUNTEER_NAME +
                "</span></p><p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi </p>" +
                "<p> Thân ái </p>" +
                "<p><sub>Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +" </sub></p></body></html>";
        }
        public static string RejectRegistrationVolunteer(VolunteerRegistrationFormViewModel volunteerModel, string error, CenterViewModel model)
        {
            return "<!DOCTYPE html><html><head><title></title></head><body data-new-gr-c-s-loaded = '14.1002.0' spellcheck = 'false'><p style = 'text-align: left;' ><strong><span style='font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p>Chào bạn, " +
                volunteerModel.LastName + " " + volunteerModel.FirstName +
                "</p><p> Chúng tôi đến từ hệ thống cứu hộ <strong>" +
                ORG_NAME +
                "</strong></p><p> Cảm ơn bạn đã đăng ký để trở thành tình nguyện viện của <strong>" +
                model.CenterName +
                "</strong>. Nhưng chúng tối rất tiếc phải thông báo rằng, <strong>" +
                model.CenterName +
                "</strong> chúng tôi đã phải từ chối đơn đăng ký của bạn với lý do như sau:</p><ul>" +
                error +
                "</ul><p> Cảm ơn sự giúp đỡ của bạn đối với hệ thống của chúng tôi.</p><p> Thân ái </p>" +
                "</span></p><p> --------------------------------------------------</p>" +
                "<p><sub> Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p><sub> Góp ý cho hệ thống qua mail: " +
                MAIL + 
                "</body></html> ";
        }
        public static string ApproveAdoption(CenterViewModel model, string petName, string username)
        {
            return "<!DOCTYPE html>" +
                "<html><head><title></title></head>" +
                "<body data-new-gr-c-s-loaded='14.1002.0' spellcheck = 'false'>" +
                "<p style = 'text-align: left;'><strong><span style = 'font-size: 22px;'> " +
                ORG_NAME +
                " </span></strong></p><p> Chào bạn, " +
                username +
                "</p><p> Chúng tôi đến từ hệ thống cứu hộ <strong> " +
                ORG_NAME +
                "</strong></p><p> Chúng tôi đại điện cho " +
                model.CenterName +
                " chúc mừng bạn, đơn đăng ký nhận nuôi của bạn đã được duyệt, bạn đã được nhận nuôi " +
                petName +
                " của " +
                model.CenterName +
                ". Chúng tôi trân trọng kính mời bạn đến trung tâm để hoàn thành các thủ tục, hồ sơ để đón bé về.</p>" +
                "<p> Thời gian: " +
                TIME_WORK +
                " </p>" +
                "<p> Địa điểm: " +
                model.Address +
                " </p>" +
                "<p> Lưu ý: Bạn cần phải đem vật dụng dùng để vận chuyển bé về, nếu bạn không hoành thành các thủ tục hoặc không đến lấy sau " +
                DEFAULT_DATE +
                " ,chúng tôi sẽ hủy kết quả của bạn và tìm người chủ mới cho bé.</p>" +
                "<p> Thân ái </p>" +
                "<p> --------------------------------------------------</p>" +
                "<p style = 'line-height: 1;' ><span style = 'font-size: 14px;'> Mọi thông tin chi tiết xin liên hệ :</span></p>" +
                "<p><span style='font-size: 14px;'> " +
                model.CenterName +
                "</span></p>" +
                "<p><span style = 'font-size: 14px;'> SDT: " +
                model.Phone +
                " </span></p>" +
                "<p><span style = 'font-size: 14px;'> Địa chỉ: " +
                model.Address +
                "</span></p>" +
                "<p> --------------------------------------------------</p><p style='line-height: 0.1;'><sub> Hệ Thống Cứu Hộ " +
                ORG_NAME +
                "</sub></p><p><sub> Góp ý cho hệ thống qua mail: " +
                MAIL +
                " </sub></p></body></html>";
        }
    }
}
