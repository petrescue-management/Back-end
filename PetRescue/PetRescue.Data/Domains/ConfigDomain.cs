
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

        #region GET TIME TO NOTIFICATION FOR FINDER FORM
        public object GetTimeToNotificationForFinderForm()
        {
            
                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToNotificationForFinderForm.json");

                var fileJson = File.ReadAllText(FILEPATH);

                var time = JObject.Parse(fileJson);
          
                return new { reNotiTime = int.Parse(time["ReNotiTime"].Value<string>()),
                    destroyNotiTime = int.Parse(time["DestroyNotiTime"].Value<string>())};           
        }
        #endregion

        #region CONFIG TIME TO NOTIFICATION FOR FINDER FORM
        public bool ConfigTimeToNotificationForFinderForm(int reNotiTime, int destroyNotiTime)
        {
            if (reNotiTime < destroyNotiTime)
            {
                string newJson = "{" +
                     "'ReNotiTime': '" + reNotiTime + "'," +
                     "'DestroyNotiTime': '" + destroyNotiTime + "'}";
                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToNotificationForFinderForm.json");

                var fileJson = File.ReadAllText(FILEPATH);

                var newConfigTime = JObject.Parse(newJson);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(newConfigTime, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(FILEPATH, output);
                return true;
            }
            return false;
        }
        #endregion

        #region GET TIME TO REMIND FOR REPORT AFTER ADOPT
        public object GetTimeToRemindForReportAfterAdopt()
        {
            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToRemindForReportAfterAdopt.json");

            var fileJson = File.ReadAllText(FILEPATH);

            var time = JObject.Parse(fileJson);

            return new { remindTime = int.Parse(time["RemindTime"].Value<string>()) };
        }
        #endregion

        #region CONFIG TIME TO REMIND FOR REPORT AFTER ADOPT
        public bool ConfigTimeToRemindForReportAfterAdopt(int RemindTime)
        {
                string newJson = "{" +
                     "'RemindTime': '" + RemindTime + "'}";

                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToRemindForReportAfterAdopt.json");

                var fileJson = File.ReadAllText(FILEPATH);

                var newConfigTime = JObject.Parse(newJson);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(newConfigTime, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(FILEPATH, output);
                return true;
        }
        #endregion
    }
}
