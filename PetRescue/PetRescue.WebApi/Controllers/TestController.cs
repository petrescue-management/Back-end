//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
////using FireSharp.Config;
////using FireSharp.Response;
//using Google.Apis.Auth.OAuth2;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using PetRescue.Data.ConstantHelper;
//using PetRescue.Data.Domains;
//using PetRescue.Data.Extensions;
//using PetRescue.Data.Uow;
//using PetRescue.Data.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace PetRescue.WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestController : BaseController
//    {
//        private readonly IHostingEnvironment _env;
//        public TestController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
//        {
//            _env = environment;
//        }
//        [HttpGet]
//        [Route("test")]
//        public async Task<IActionResult> TestAsync()
//        {
//            try
//            {
//                string path = _env.ContentRootPath;
//                path += "\\service-accounts.json";
//                FirebaseApp app = null;
//                try
//                {
//                    app = FirebaseApp.Create(new AppOptions()
//                    {
//                        Credential = GoogleCredential.FromFile(path)
//                    }, "PetRescue");
//                }
//                catch
//                {
//                    app = FirebaseApp.GetInstance("PetRescue");
//                }
//                var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);
//                Message message = new Message()
//                {
//                    Notification = new Notification
//                    {
//                        Title = "My push notification title",
//                        Body = "Content for this push notification"
//                    },
//                    Data = new Dictionary<string, string>()
//                    {
//                        { "AdditionalData1", "data 1" },
//                        { "AdditionalData2", "data 2" },
//                        { "AdditionalData3", "data 3" },
//                    },
//                };
//                //FirebaseMessaging.GetMessaging(app).SubscribeToTopicAsync
//                var result = await fcm.SendAsync(message);
//                app.Delete();
//                //var deviceTk = "co1sDdXd1CpV0KVclkBqdC:APA91bHU3pnB5cruMojrn4sBn-jUP7cbLHtIrdB2E9SJmBIBNWaPgronZmRsm0-a2DS95Cr7viPHOoHc5C_nQu3LZ1MfK6KREYstNQ2lUAV-ZVuN_Zt7T-Us2haJMl57CBq-jDuKiVnk";
//                //var txtMes = "hello";
//                //var title = "Test Notification";
//                //Notification1.SendNotification(deviceTk, title, txtMes);
//                return Success(result);
//            }
//            catch (Exception ex)
//            {
//                return Error(ex.Message);
//            }
//        }
//        [HttpGet]
//        [Route("test2")]
//        public async Task<IActionResult> TestTopicAsync()
//        {
//            try
//            {
//                string[] listToken = { "c6pSCVkOWDXdVfDPvc9gTZ:APA91bGLG61l3ziiQbFLbhOVDjuUJAZtFweBYOpBYKh37kGxqU4P5weRUzqG-Xug3D0smK8Uf8cK-P3IDiOZ3ePLYDNv8M-qBdz2TWpuqB2eOURm8Z3xF4zJf8wixspS-NBjMF7Rxlw-" };
//                string centerId = "9f447931-9368-4d9d-acba-4ac44ff44e0d";
//                Message message = new Message()
//                {
//                    Notification = new Notification
//                    {
//                        Title = "My push notification title",
//                        Body = "Content for this push notification"
//                    },
//                    Data = new Dictionary<string, string>()
//                    {
//                        { "AdditionalData1", "data 1" },
//                        { "AdditionalData2", "data 2" },
//                        { "AdditionalData3", "data 3" },
//                    },
//                    Topic = centerId
//                };
//                string path = _env.ContentRootPath;
//                //path += "\\service-accounts.json";
//                var firebaseExtensions = new FireBaseExtentions();
//                var app = firebaseExtensions.GetFirebaseApp(path);
//                var fcm = FirebaseMessaging.GetMessaging(app);
//                await fcm.SubscribeToTopicAsync(listToken, centerId);
//                var result = await fcm.SendAsync(message);
//                app.Delete();
                

//                return Success(result);
//            }
//            catch(Exception ex)
//            {
//                return Error(ex.Message);
//            }
//        }
//        [HttpGet]
//        [Route("test3")]
//        public async Task<IActionResult> TestGoogleMapAsync()
//        {
//            try
//            {
//                var origins = "10.80101854518685, 106.7887330986729";
//                var destination1 = "10.804770321373686, 106.79124364631357";
//                var destination4 = "47.751076, -120.740135";
//                var destination2 = "10.841956917312132, 106.80932035873482";
//                var destination3 = "47.751076, -120.740135";
//                var units = "basic";
//                var key = "AIzaSyAZ4pja68qoa62hCzFdlmAu30iAb_CgmTk";
//                var url = "https://maps.googleapis.com/maps/api/distancematrix/json?" +
//                    //"units=" + units +
//                    "units=imperial" +
//                    "&origins=" + origins +
//                    "&destinations=" + destination1 + "|" + destination4 + "|" + destination3 + "|" + destination2 +
//                    "&key=" + key;
//                //WebRequest request = WebRequest.Create(url);

//                //WebResponse response = request.GetResponse();

//                //Stream data = response.GetResponseStream();

//                //StreamReader reader = new StreamReader(data);
//                //string responseFromServer = reader.ReadToEnd();
//                //MapModel list = JsonConvert.DeserializeObject<MapModel>(responseFromServer);
//                var center = _uow.GetService<CenterDomain>();
//                var extension = new GoogleMapExtensions();
//                //var result = extension.FindShortestCenter(origins, center.GetListCenter());
//                //return Success(result);
//                return null;
//            }
//            catch(Exception ex)
//            {
//                return Error(ex.Message);
//            }
//        }
//    }
//}
