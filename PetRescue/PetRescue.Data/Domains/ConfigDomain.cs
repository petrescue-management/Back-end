
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
                    reNotiTimeForRescue = int.Parse(time["ReNotiTimeForRescue"].Value<string>()),
                    destroyNotiTimeForRescue = int.Parse(time["DestroyNotiTimeForRescue"].Value<string>()),
                    remindTimeAfterAdopt = int.Parse(time["RemindTimeAfterAdopt"].Value<string>()),
                    imageForFinder = int.Parse(time["ImageForFinder"].Value<string>()),
                    imageForPicker = int.Parse(time["ImageForPicker"].Value<string>()),
                    nearestDistance = double.Parse(time["NearestDistance"].Value<string>())
                };           
        }
        #endregion

        #region CONFIG TIME TO NOTIFICATION
        public bool ConfigTimeToNotification(int reNotiTime, int destroyNotiTime, int remindTime, int imgFinder, int imgPicker, double nearestDistance)
        {
            if (reNotiTime < destroyNotiTime)
            {
                string FILEPATH =
                    Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");


                string newJson = "{" +
                  "'ReNotiTimeForRescue': '" + reNotiTime + "'," +
                  "'DestroyNotiTimeForRescue': '" + destroyNotiTime + "'," +
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
