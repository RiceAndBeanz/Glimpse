using Glimpse.Core.Model;
using Glimpse.Core.Model.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Services.General
{
    class GoogleWebService
    {
        //https://maps.googleapis.com/maps/api/directions/json?origin=Disneyland&destination=Universal+Studios+Hollywood4&key=YOUR_API_KEY

        private readonly string baseUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?";

        //JOSEPH KEY
         //private readonly string key = "&key=AIzaSyCh-7urF7EEXVqH7gePdjvgx3Pjp4qZEvE";
        //private readonly string key = "&key=AIzaSyB3IwKBpvbadKnZLd7QK4OhnBt6G3-1uDU";

        //Eric's KEY
        private readonly string key = "AIzaSyBRO7YkJf3hzbSen3tT2vfWZttdE4MJ3xk";

        //SAM KEY
        // private readonly string key = "&key=AIzaSyBgkvg2Yy7YyphYP3l_Bim8ZtwzjfSuoYM";


        private string result;

        /// <summary>
        /// get response for 1 origin and 1 destinaton
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public async Task<DistanceMatrix> GetIndividualDurationResponse(Location origin, Location destination)
        {
            string url = buildUrl(origin, destination);
            return await GetResponse(url);
        }


        /// <summary>
        /// get response for 1 origin and list of destinations (max 25 atm need to handle more than 25, web request is limited to 25 destination)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public async Task<DistanceMatrix> GetMultipleDurationResponse(Location origin, List<Location> destination)
        {
            string url = buildUrl(origin, destination);
            return await GetResponse(url);
        }

        /// <summary>
        /// does the actual http request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<DistanceMatrix> GetResponse(string url)
        {            
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseAsString = await response.Content.ReadAsStringAsync();
            DistanceMatrix result = JsonConvert.DeserializeObject<DistanceMatrix>(responseAsString);

            return result;
        }      

        /// <summary>
        /// build url for single destination format
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private string buildUrl(Location origin, Location destination)
        {
            return baseUrl + "origins=" + origin.Lat + "," + origin.Lng + "&destinations=" + destination.Lat + "," + destination.Lng + key;
        }

        /// <summary>
        /// build url for multiple destination format
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        private string buildUrl(Location origin, List<Location> destinations)
        {
            string url = baseUrl + "origins=" + origin.Lat + "," + origin.Lng;

            url = url + "&destinations=";

            var count = destinations.Count;

            foreach(Location location in destinations)
            {
                url = url + location.Lat + "," + location.Lng;
                if(--count > 0)
                {
                    url = url + "|";
                }              

            }

            url = url + "&key=" + key;

            return url;             
        }
    }
}
