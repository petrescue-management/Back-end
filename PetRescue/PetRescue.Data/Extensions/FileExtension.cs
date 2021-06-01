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
            bool check = true;
            while (check) {
                try
                {
                    string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "VolunteersLocation.json");
                    var json = File.ReadAllText(FILEPATH);
                    result = JsonConvert.DeserializeObject<Dictionary<Guid, UserLocation>>(json);
                    check = false;
                }
                catch {
                    check = true;
                }
            }
            return result;
        }
        public bool WriteLocationFile(Dictionary<Guid, UserLocation> result)
        {
            bool check = false;
            while (!check) {
                try
                {
                    string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "VolunteersLocation.json");
                    var json = JsonConvert.SerializeObject(result);
                    File.WriteAllText(FILEPATH, json);
                    check = true;
                }
                catch {
                    check = false;
                }
            }
            return check;
        }
    }
}
