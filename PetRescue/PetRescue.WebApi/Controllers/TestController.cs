using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FireSharp.Config;
using FireSharp.Response;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public TestController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> TestAsync()
        {
            try
            {
                string path = _env.ContentRootPath;
                path += "\\service-accounts.json";
                FirebaseApp app = null;
                try
                {
                    app = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(path)
                    }, "PetRescue");
                }
                catch
                {
                    app = FirebaseApp.GetInstance("PetRescue");
                }
                var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = "My push notification title",
                        Body = "Content for this push notification"
                    },
                    Data = new Dictionary<string, string>()
                    {
                        { "AdditionalData1", "data 1" },
                        { "AdditionalData2", "data 2" },
                        { "AdditionalData3", "data 3" },
                    },
                    
                    Token = "dnegPYwnNlreULLtzT0ao8:APA91bEL7-VXXE_GuENU26gNHAmR-lUfTnYYTNPgeWPtbOHM9-Ir4vHFxaO4k2YoQ5TKz4PtT9340lBbDZ80Qyve3eIzDUrVvoy-PbciJqFIac8vPPPTP8G5YVoSPF2VHG1Qod8hdjmn",
                };
                //FirebaseMessaging.GetMessaging(app).SubscribeToTopicAsync
                var result = await fcm.SendAsync(message);

                //var deviceTk = "co1sDdXd1CpV0KVclkBqdC:APA91bHU3pnB5cruMojrn4sBn-jUP7cbLHtIrdB2E9SJmBIBNWaPgronZmRsm0-a2DS95Cr7viPHOoHc5C_nQu3LZ1MfK6KREYstNQ2lUAV-ZVuN_Zt7T-Us2haJMl57CBq-jDuKiVnk";
                //var txtMes = "hello";
                //var title = "Test Notification";
                //Notification1.SendNotification(deviceTk, title, txtMes);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("test2")]
        public async Task<IActionResult> TestDataBase()
        {
            try
            {
                var client = new FireSharp.FirebaseClient(FCMConfig.config);
                var data = new Dictionary<string,object>();
                var content = new Dictionary<string, string>();
                content.Add("Name", "test1");
                content.Add("Description", "test2");
                content.Add("Subdescription", "test3");
                PushResponse response = client.Push("Centers",content);
                
                FirebaseResponse response1 = client.Get("Centers");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                return Success(data1);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        public class Notification1
        {

            public static string SendNotification(string DeviceToken, string title, string msg)
            {
                var result = "-1";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(FCMConfig.WEBADDR);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", FCMConfig.SERVERKEY));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", FCMConfig.SENDERID));
                httpWebRequest.Method = "POST";

                var payload = new
                {
                    to = DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = msg,
                        title = title
                    },
                };
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(payload);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
        }
    }
}
