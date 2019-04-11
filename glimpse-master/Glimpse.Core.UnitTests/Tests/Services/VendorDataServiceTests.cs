using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.Services.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.UnitTests.Tests.Services
{
    [TestClass]
    public class VendorDataServiceTests
    {
        private VendorDataService _vds;

        private readonly string _testEmail = "unitTestEmail@gmail.com";
        private readonly string _testPassword = "unitTestPassword";
        
        [TestInitialize]
        public void Initialize()
        {
            VendorRepository vendorRepo = new VendorRepository();
            _vds = new VendorDataService(vendorRepo);
        }

        [TestMethod]
        public async Task CreateVendor_DuplicateEmail_Doesnt_Create()
        {
            //arrange
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


            //act
            bool firstSignUpSuccess = await _vds.SignUp(vendor);

            //signup twice
            bool secondSignUpSuccess = await _vds.SignUp(vendor);

            //assert
            //check that 2 user with same email were not created

            List<Vendor> allVendors = await _vds.GetVendors();
            List<Vendor> vendorWithTestEmail = allVendors.FindAll(v => v.Email == _testEmail);

            //clean up
            await _vds.DeleteVendor(vendorWithTestEmail[0]);

            Assert.IsTrue(firstSignUpSuccess);
            Assert.IsFalse(secondSignUpSuccess);
            Assert.IsTrue(vendorWithTestEmail.Count == 1);


        }


        [TestMethod]
        public async Task CheckIfVendorExists_returns_valid()
        {
            //arrange
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

            //act
            await _vds.SignUp(vendor);

            //assert
            Assert.IsTrue(await _vds.CheckIfVendorExists(_testEmail));

            //clean up

            await _vds.DeleteVendor(vendor);
        }

        [TestMethod]
        public async Task CheckIfPasswordAndSalt_AreNull()
        {
            //arrange
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

            //act
            await _vds.SignUp(vendor);

            //assert
            Vendor vendorFromDb = await _vds.SearchVendorByEmail(_testEmail);
            Assert.IsTrue(await _vds.CheckIfVendorExists(_testEmail));
            Assert.IsNull(vendorFromDb.Password);
            Assert.IsNull(vendorFromDb.Salt);

            //clean up

            await _vds.DeleteVendor(vendorFromDb);
        }

        [TestMethod]
        public async Task TestVendorLocation_IsUnique()
        {
            //Create first vendor
            Vendor vendor1 = new Vendor
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

            //Sign the first vendor
            await _vds.SignUp(vendor1);

            //Create second vendor
            Vendor vendor2 = new Vendor
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

            //Sign up the second vendor
            //This should fail due to exact duplicate of location coordinates Lat Lng
            Assert.IsFalse(await _vds.SignUp(vendor2));
            Assert.IsTrue(vendor1.Location.Lat == vendor2.Location.Lat && vendor2.Location.Lng == vendor1.Location.Lng);

            //clean up
            await _vds.DeleteVendor(vendor1);
            await _vds.DeleteVendor(vendor2);

        }

    }
}
