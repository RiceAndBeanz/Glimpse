using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Glimpse.Core.Services.General;

namespace Plugin.RestClient
{
    /// <summary>
    ///     RestClient implements methods for calling CRUD operations
    ///     using HTTP.
    /// </summary>
    public class RestClient<T> {

        //http://glimpsews.azurewebsites.net/api/
        //http://glimpseservices.azurewebsites.net/api/
        //http://10.0.3.2/Glimpse/api/
        //http://localhost/Glimpse/api/
        //http://glimpseofficial.azurewebsites.net/api/

        private readonly string WebServiceUrl = "http://glimpseofficial.azurewebsites.net/api/" + typeof(T).Name + "s/";

        /// <summary>
        /// get request
        /// </summary>
        /// <param name="parameters">Dictionary is used to pass parameter, first string is name of parameter, second is the value</param>
        /// <returns></returns>
        public async Task<List<T>> GetAsync(Dictionary<string,string> parameters = null)
        {
            var httpClient = new HttpClient();
            string request = WebServiceUrl;
            bool firstParam = false;
            if(parameters != null)
            {
                foreach (KeyValuePair<string, string> entry in parameters)
                {
                    if (!firstParam)
                    {
                        request = request.Remove(request.Length - 1) + "?";
                        firstParam = true;
                    }                      
                    else
                        request = request + "&";

                    request = request +entry.Key + "=" + entry.Value;
                }
            }

           // Log.Info(string.Format("Request: " + request.ToString()));

            var json = await httpClient.GetStringAsync(request);
            var taskModels = JsonConvert.DeserializeObject<List<T>>(json);

           // if (taskModels != null)
           //     Log.Info("Returning: " + taskModels.ToList());

            return taskModels;
        }


        public async Task<List<T>> GetByIdAsync(int id)
        {
            //Log.Info(string.Format("Getting by id: {0}", id));

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + id);

            var taskModels = JsonConvert.DeserializeObject<List<T>>(json);

           // if (taskModels != null)
           //     Log.Info(string.Format("Returning: {0}", taskModels.ToList()));      

            return taskModels;
        }

        public async Task<List<T>> GetWithFilter(string filter)
        {
            //Log.Info(string.Format("Attemping to get by filter: {0}", filter));

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "filter/" + filter);

            var taskModels = JsonConvert.DeserializeObject<List<T>>(json);

           // if (taskModels != null)
            //    Log.Info(string.Format("Returning: {0}", taskModels.ToList()));

            return taskModels;
        }


        public async Task<T> GetByKeyword(string keyword, bool slashRequired = false)
        {
            //Log.Info(string.Format("Attemping to get by keyword: {0}", keyword));
            var httpClient = new HttpClient();

            string request = WebServiceUrl + "Search/" + keyword;

            if (slashRequired)
                request = request + "/";
           // Log.Info(string.Format("With request: {0}", request));
            var json = await httpClient.GetStringAsync(request);

            var taskModel = JsonConvert.DeserializeObject<T>(json);

           // if (taskModel != null)
           //       Log.Info(string.Format("Returning: {0}", taskModel.ToString()));

            return taskModel;
        }

        public async Task<bool> PostAsync(T t)
        {
            //Log.Info(string.Format("Attemping to post into database: {0}", t.ToString()));

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);

           /* if (!result.IsSuccessStatusCode)
                Log.Error("Posting was unsuccessful");
            else
            {
                Log.Info("Posting was succesful");
            }
           */
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(int id, T t)
        {
            //Log.Info(string.Format("Attemping to update {0} db with id: {1}",t.ToString(),id));

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WebServiceUrl + id, httpContent);

            /*if (!result.IsSuccessStatusCode)
                Log.Error("Update was unsuccessful");
            else
            {
                Log.Info("Update was succesful");
            }
            */
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, T t)
        {
            //Log.Info(string.Format("Attemping to delete {0} db with id: {1}", t.ToString(), id));

            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WebServiceUrl + id);

           /* if (!response.IsSuccessStatusCode)
                Log.Error("Deletion was unsuccessful");
            else
            {
                Log.Info("Deletion was succesful");
            }
            */
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetIdAsync(string email)
        {
            //Log.Info(string.Format("Getting by email: {0}", email));

            var httpClient = new HttpClient();

            //Log.Info("Web Service URL: " + WebServiceUrl + "Search/" + email + "/"); 
             
            var json = await httpClient.GetStringAsync(WebServiceUrl + "Search/" + email + "/");

            var list = JsonConvert.DeserializeObject<List<dynamic>>(json);

            var  obj = list.FirstOrDefault();

            //Log.Info("Returning: " + obj["Id"]);

            return (int)obj["Id"];
        }

        public async Task<bool> Authenticate(T t)
        {
            string request = WebServiceUrl.Substring(0, WebServiceUrl.IndexOf("api")) + "authenticate";
            //string request = "http://localhost/Glimpse/authenticate/";

            //Log.Info("Request: " + request.ToString());

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
           
            var result = await httpClient.PostAsync(request, httpContent);

            /*if (!result.IsSuccessStatusCode)
                Log.Error("Authentication was unsuccessful");
            else
            {
                Log.Info("Authentication was succesful");
            }*/

            return result.IsSuccessStatusCode;                  

        }


    }
}
