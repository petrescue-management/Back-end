//using FireSharp.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ConstantHelper
{
    public class FCMConfig
    {
        public static string SERVERKEY = "AAAATfpIqho:APA91bGH_SXMpPTnS6_EngJszoYMbci31D8KNo_a2m4EoZ1YKV3pq2ua68doud12RibIj42zImGd4sP6t4mtArnvbJqIPD0x4vkoMdOMjIr-wwO2tmQQG1PCf7-rCV9bxwOfP3U8Lb0d";
        public static string SENDERID = "334911547930";
        public static string WEBADDR = "https://fcm.googleapis.com/fcm/send";
        public static string FILE_NAME = "\\service-accounts.json";
        public static string FIREBASE_INSTANCE = "PetRescue";
    }
    public class ApplicationNameHelper
    {
        public const string SYSTEM_ADMIN_APP = "Petrescue.app.systemadmin";
        public const string MANAGE_CENTER_APP = "Petrescue.app.managercenter";
        public const string USER_APP = "Petrescue.app.user";
        public const string VOLUNTEER_APP = "Petrescue.app.volunteer";
    }
    public class NotificationTitleHelper
    {
        public const string NEW_REGISTRATION_CENTER_FORM_TITLE = "Bạn có thông báo đăng ký trung tâm";
        public const string NEW_REGISTRATON_ADOPTION_FORM_TITLE = "Bạn có thông báo đăng ký nhận nuôi thú cưng";
        public const string APPROVE_ADOPTION_FORM_TITLE = "Bạn có thông báo tình trạng đăng ký nhận nuôi";
        public const string REJECT_ADOPTION_FORM_TITLE = "Bạn có thông báo tình trạng đăng ký nhận nuôi";
        public const string NEW_RESCUE_FORM_TITLE = "Bạn có một yêu cầu cứu hộ";
        public const string NEW_VOLUNTEER_FORM_TITLE = "Bạn có thông báo đăng ký tình nguyện viên";
        public const string RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_TITLE = "Tình Nguyện Viên đã tiếp nhận";
        public const string FINDER_FORM_OUT_DATE_TITLE = "Đơn cứu hộ bị quá hạn";
        public const string ALERT_AFTER_ADOPTION_TITLE = "Bạn có một thông báo bổ sung";
        public const string USER_DONT_GET_PET_TITLE = "Bạn có thông báo tình trạng đăng ký nhận nuôi";
    }
    public class NotificationBodyHelper
    {
        public const string NEW_REGISTRATION_CENTER_FORM_BODY = "Có đơn đăng ký thành Trung Tâm cần xử lý";
        public const string NEW_REGISTRATION_ADOPTION_FORM_BODY = "Có đơn đăng ký nhận nuôi thú cưng cần xử lý";
        public const string APPROVE_ADOPTION_FORM_BODY = "Bạn đã được nhận nuôi một con thú cưng";
        public const string HAVE_FOUND_ADOPTION_FORM_TITLE = "Xin lỗi, trung tâm đã tìm ra người phù hợp";
        public const string REJECT_ADOPTION_FORM_TITLE = "Xin lỗi, bạn không phù hợp với thú cưng này";
        public const string NEW_RESCUE_FORM_BODY = "Có đơn cứu hộ cần được xử lý";
        public const string NEW_VOLUNTEER_FORM_BODY = "Có đơn đăng ký tình nguyện viên cần được xử lý";
        public const string RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_BODY = "Tình Nguyện Viên đã nhận được yêu cầu cứu hộ";
        public const string FINDER_FORM_OUT_DATE_BODY = "Đơn cứu hộ của bạn đã quá thời gian";
        public const string ALERT_AFTER_ADOPTION_BODY = "Bạn cần cập nhật trình trạnh cho bé";
        public const string USER_DONT_GET_PET_BODY = "Bạn có thông báo tình trạng đăng ký nhận nuôi";
    }
}
