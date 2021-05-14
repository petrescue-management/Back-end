
using Newtonsoft.Json.Linq;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class ConfigDomain : BaseDomain
    {
        public ConfigDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region GET TIME TO NOTIFICATION
        public object GetTimeToNotification()
        {
            
                string FILEPATH = 
                Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");

                var fileJson = File.ReadAllText(FILEPATH);

                var time = JObject.Parse(fileJson);
          
                return new {
                    reNotiTimeForOnlineRescue = int.Parse(time["ReNotiTimeForOnlineRescue"].Value<string>()),
                    reNotiTimeForAllRescue = int.Parse(time["ReNotiTimeForAllRescue"].Value<string>()),
                    notiTimeForDestroyRescue = int.Parse(time["NotiTimeForDestroyRescue"].Value<string>()),
                    remindTimeAfterAdopt = int.Parse(time["RemindTimeAfterAdopt"].Value<string>()),
                    imageForFinder = int.Parse(time["ImageForFinder"].Value<string>()),
                    imageForPicker = int.Parse(time["ImageForPicker"].Value<string>()),
                    nearestDistance = double.Parse(time["NearestDistance"].Value<string>())
                };           
        }
        #endregion

        #region CONFIG TIME TO NOTIFICATION
        public bool ConfigTimeToNotification(int reNotiTimeForOnline, int reNotiTimeForAll, int notiTimeForDestroy, 
            int remindTime, int imgFinder, int imgPicker, double nearestDistance)
        {
            if (reNotiTimeForOnline < reNotiTimeForAll 
                && reNotiTimeForAll < notiTimeForDestroy 
                && reNotiTimeForOnline < notiTimeForDestroy)
            {
                string FILEPATH =
                    Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");


                string newJson = "{" +
                  "'ReNotiTimeForOnlineRescue': '" + reNotiTimeForOnline + "'," +
                  "'ReNotiTimeForAllRescue': '" + reNotiTimeForAll + "'," +
                  "'NotiTimeForDestroyRescue': '" + notiTimeForDestroy + "'," +
                  "'RemindTimeAfterAdopt': '" + remindTime + "'," +
                  "'NearestDistance': '" + nearestDistance + "'," +
                  "'ImageForFinder': '" + imgFinder + "'," +
                  "'ImageForPicker': '" + imgPicker  + "'}";

                var newConfigTime = JObject.Parse(newJson);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(newConfigTime, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(FILEPATH, output);
                return true;
            }
            return false;
        }
        #endregion

    }
}
