
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

        #region CONFIG TIME TO NOTIFICATION FOR FINDER FORM
        public bool ConfigTimeToNotificationForFinderForm(int ReNotiTime, int DestroyNotiTime)
        {
            if (ReNotiTime < DestroyNotiTime)
            {
                string newJson = "{" +
                     "'ReNotiTime': '" + ReNotiTime + "'," +
                     "'DestroyNotiTime': '" + DestroyNotiTime + "'}";
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
    }
}
