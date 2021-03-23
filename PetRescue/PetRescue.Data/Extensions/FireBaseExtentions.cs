using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using PetRescue.Data.ConstantHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public class FireBaseExtentions
    {
       
        public FireBaseExtentions()
        {
        }
        public  FirebaseApp GetFirebaseApp (string path)
        {
            var realPath = path + FCMConfig.FILE_NAME;
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(realPath)
            }, FCMConfig.FIREBASE_INSTANCE);
            var app = FirebaseApp.GetInstance(FCMConfig.FIREBASE_INSTANCE);
            return app;
        }
    }
}
