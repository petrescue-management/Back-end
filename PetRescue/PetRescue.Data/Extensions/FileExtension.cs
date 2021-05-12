using Newtonsoft.Json;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public class FileExtension
    {
        public FileExtension()
        {
        }
        public Dictionary<Guid,UserLocation> GetAvailableVolunteerLocation()
        {
            var result = new Dictionary<Guid, UserLocation>();
            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "VolunteersLocation.json");
            var json = File.ReadAllText(FILEPATH);
            result = JsonConvert.DeserializeObject<Dictionary<Guid, UserLocation>>(json);
            return result;
        }
        public bool WriteLocationFile(Dictionary<Guid, UserLocation> result)
        {
            try
            {
                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "VolunteersLocation.json");
                var json = JsonConvert.SerializeObject(result);
                File.WriteAllText(FILEPATH, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
