using Newtonsoft.Json;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public class GoogleMapExtensions
    {
        public List<CenterDistanceModel> FindListShortestCenter(string origin, List<CenterLocationModel> centers)
        {
            var destinationStr = "";
            var listCenterId = new List<string>();
            foreach (var center in centers)
            {
                var temp = center.Lat + ", " + center.Lng;
                destinationStr += temp + "|";
                listCenterId.Add(center.CenterId.ToString());
            }
            if (destinationStr.Length != 0)
            {
                destinationStr = destinationStr.Remove(destinationStr.LastIndexOf("|"));
            }
            var urlRewriting = GoogleMapConst.URL +
                "units=" + GoogleMapConst.UNITS +
                "&origins=" + origin +
                "&destinations=" + destinationStr +
                "&key= " + GoogleMapConst.API_KEY;
            WebRequest request = WebRequest.Create(urlRewriting);

            WebResponse response = request.GetResponse();

            Stream data = response.GetResponseStream();

            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            MapModel list = JsonConvert.DeserializeObject<MapModel>(responseFromServer);
            var result = new List<CenterDistanceModel>();
            var listDistance = list.rows[0].elements;
            for (int index = 0; index < listCenterId.Count; index++)
            {
                if (listDistance[index].distance != null)
                {
                    result.Add(new CenterDistanceModel
                    {
                        CenterId = listCenterId[index],
                        Value = listDistance[index].distance.value
                    });
                }
            }
            result.Sort();
            return result;
        }
    }
    
}
