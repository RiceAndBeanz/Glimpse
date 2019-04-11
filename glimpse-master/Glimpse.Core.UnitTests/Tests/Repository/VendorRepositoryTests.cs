using System;
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Glimpse.Core.UnitTests.Tests.Repository
{
    [TestClass]
    public class VendorRepositoryTests
    {
        IVendorRepository repository;
        private readonly string _testEmail = "unitTestEmail@gmail.com";
        private readonly string _testPassword = "unitTestPassword";

        [TestInitialize]
        public void Initialize()
        {
            repository = new VendorRepository();
        }

        [TestMethod]
        public async Task Test_GetVendors_Return_All_Users()
        {
            //Act
            var vendors = await repository.GetVendors();

            //Assert
            Assert.AreNotEqual(0, vendors.Count);
        }

        [TestMethod]
        public async Task Test_PostVendors_Creates_Vendor()
        {
            //arrange
            var vendorsBefore = await repository.GetVendors();
            var vendorsCountBefore = vendorsBefore.Count;

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
            bool signUpSuccess = await repository.PostVendor(vendor);

            //Assert
            var vendorsAfter = await repository.GetVendors();
            var vendorsCountAfter = vendorsAfter.Count;

            var difference = vendorsCountAfter - vendorsCountBefore;


            Assert.IsTrue(difference == 1);
            Assert.IsTrue(signUpSuccess);

            //clean up
            List<Vendor> vendorWithTestEmail = vendorsAfter.FindAll(v => v.Email == _testEmail);
            await repository.DeleteVendor(vendorWithTestEmail[0]);
        }

    }
    
}
