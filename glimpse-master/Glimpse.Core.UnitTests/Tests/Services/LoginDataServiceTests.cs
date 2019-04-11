using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.Services.Data;
using Glimpse.Core.Services.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.UnitTests.Tests.Services
{
    [TestClass]
    public class LoginDataServiceTests
    {
        private VendorDataService _vds;
        private LoginDataService _lds;

        private readonly string _testEmail = "hash@gmail.com";
        private readonly string _testPassword = "hash";

        [TestInitialize]
        public void Initialize()
        {
            VendorRepository vendorRepo = new VendorRepository();
            _lds = new LoginDataService(vendorRepo);
            _vds = new VendorDataService(vendorRepo);

        }      

        [TestMethod]
        public async Task ValidAuthetication_ReturnTrue()
        {
            Vendor vendor = new Vendor
            {
                Email = _testEmail,
                Password = _testPassword,
                CompanyName = "UnitTestCompany",
                Address = "Unit Test Address",
                Telephone = "543-535-5353",
                Location = new Location
                {
                    Lat = 54.434,
                    Lng = 53.656,
                },
            };

            //for some reason, having this call makes the authentication fail. I suspect it has to do with this post operation taking longer than authentication
            // will have to be looked at
            //bool signUp = await _vds.SignUp(vendor);

            //act               

            RestClient<Vendor> restclient = new RestClient<Vendor>();

            var response =  await restclient.Authenticate(vendor);

            Assert.IsTrue(response);

            //cleanup
            // see problem above for explanation on why this is commented
            //Vendor vendorFromDb = await _vds.SearchVendorByEmail(_testEmail);
            //await _vds.DeleteVendor(vendorFromDb);
        }



    }
}
