using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Glimpse.Core.UnitTests.Tests.Services
{
    [TestClass]
    public class LoadPromotionTests
    {
        
        [TestMethod]
        public async Task GetVendorIdAsyncTest()
        {
            var id = await GetVendorIdAsync("");

            Assert.IsTrue(id == 1);
        }

        public async Task<int> GetVendorIdAsync(string username)
        {
            var httpClient = new HttpClient();

            var json = await Task.FromResult("[{\"Id\":1,\"FirstName\":\"a\"}]"); //await httpClient.GetStringAsync("");

            var list = JsonConvert.DeserializeObject<List<dynamic>>(json);

            var obj = list.FirstOrDefault();

            return (int)obj["Id"];
        }
    }

}
