﻿//using FireSharp.Config;
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
        //public static FirebaseConfig  config = new FirebaseConfig
        //{
        //    AuthSecret = "svKuaskcvYZpeXTo6P1q3YpfGuR8GtgxwpMSLAVK",
        //    BasePath = "https://pet-rescue-fb-default-rtdb.firebaseio.com/"
        //};
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
        public const string NEW_REGISTRATION_CENTER_FORM_TITLE = "You have a new center registration";
        public const string NEW_REGISTRATON_ADOPTION_FORM_TITLE = "You have a new adoption registration";
        public const string APPROVE_ADOPTION_FORM_TITLE = "You have new adoption pet";
        public const string NEW_RESCUE_FORM_TITLE = "You have a new Rescue Form";
    }
    public class NotificationBodyHelper
    {
        public const string NEW_REGISTRATION_CENTER_FORM_BODY = "New center registration form is created";
        public const string NEW_REGISTRATION_ADOPTION_FORM_BODY = "New adoption registration form is created";
        public const string APPROVE_ADOPTION_FORM_BODY = "you have adoption new pet";
        public const string NEW_RESCUE_FORM_BODY = "You have a new Rescue Form";
    }
    public class NotificationManagerType
    {
        public const int NEW_ADOPTION_REGISTRATION_FORM_BE_CREATED = 2;
        public const int NEW_RESCUE_FORM_BE_CREATED = 1;
    }
}
