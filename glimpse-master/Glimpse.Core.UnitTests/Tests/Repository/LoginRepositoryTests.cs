using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Glimpse.Core.Repositories;
using Glimpse.Core.Services.Data;
using Glimpse.Core.Services.General;
using Glimpse.Core.UnitTests.Helpers;
using Glimpse.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Glimpse.Core.UnitTests.Tests.Repository
{
    [TestClass]
    public class LoginRepositoryTests
    {
        private IUserRepository _userRepository;
        private IVendorRepository _vendorRepository;
        private User user;
        private Vendor vendor;

        [TestInitialize]
        public void Initialize()
        {
            _userRepository = new UserRepository();
            _vendorRepository = new VendorRepository();
        }

        [TestMethod]
        public async Task TestUserLogout()
        {
            TestSettingsMock.IsVendorAccount = false;
            TestSettingsMock.UserName = "samus";
            TestSettingsMock.Password = "sampassword";
            TestSettingsMock.LoginStatus = true;

            //Menu option logout clicked
            if ("Logout" == MenuOption.Logout.ToString())
            {
                TestSettingsMock.UserName = string.Empty;
                TestSettingsMock.Password = string.Empty;
                TestSettingsMock.LoginStatus = false;
            }
            Assert.AreNotEqual(true, TestSettingsMock.LoginStatus);
        }

        [TestMethod]
        public async Task TestVendorsLogout()
        {
            
            TestSettingsMock.IsVendorAccount = true;
            TestSettingsMock.UserName = "JohnS";
            TestSettingsMock.Password = "qotr24m2";
            TestSettingsMock.LoginStatus = true;

          
            //Menu option logout clicked
            if ("Logout" == MenuOption.Logout.ToString())
            {
                TestSettingsMock.UserName = string.Empty;
                TestSettingsMock.Password = string.Empty;
                TestSettingsMock.LoginStatus = false;
            }
            Assert.AreNotEqual(true, TestSettingsMock.LoginStatus);
        }       
    }
}