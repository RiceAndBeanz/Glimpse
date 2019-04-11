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
    public class UserDataServiceTests
    {
        private UserDataService _uds;

        private readonly string _testEmail = "Email@gmail.com";
        private readonly string _testPassword = "unitTestPassword";       

        [TestInitialize]
        public void Initialize()
        {
            UserRepository userRepo = new UserRepository();           
            _uds = new UserDataService(userRepo);
        }

        [TestMethod]
        public async Task CreateUser_DuplicateEmail_Doesnt_Create()
        {
            //arrange
            User user = new User
            {
                Email = _testEmail,
                Password = _testPassword               
            };


            //act
            await _uds.SignUp(user);

            //signup twice
            await _uds.SignUp(user);

            //assert
            //check that 2 user with same email were not created

            List<User> allUsers = await _uds.GetUsers();
            List<User> userWithTestEmail = allUsers.FindAll(u => u.Email == _testEmail);

            Assert.IsTrue(userWithTestEmail.Count == 1);

            //clean up

            await _uds.DeleteUser(userWithTestEmail[0]);


           

        }

    }
}
