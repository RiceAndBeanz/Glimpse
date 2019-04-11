using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.UnitTests.Mocks;
using System.Threading.Tasks;
using Glimpse.Core.Repositories;
using Glimpse.Core.Model;

namespace Glimpse.Core.UnitTests.Tests.Repository
{
    [TestClass]
    public class UserRepositoryTests
    {
        IUserRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            repository = new UserRepository(); ;
        }

        [TestMethod]
        public async Task Test_GetUsers_Return_All_Users()
        {
            //Act
            var users = await repository.GetUsers();

            //Assert
            Assert.AreNotEqual(0, users.Count);
        }

        [TestMethod]
        public async Task Test_PostUsers_Creates_User()
        {
            //arrange
            var usersBefore = await repository.GetUsers();
            var usersCountBefore = usersBefore.Count;
            User user = new User
            {               
                Email = "unittest@gmail.com",
                Password = "mypassword",
                Salt = "salt",
            };

            //act 
             await repository.PostUser(user);

            //Assert
            var usersAfter = await repository.GetUsers();
            var usersCountAfter = usersAfter.Count;

            var difference = usersCountAfter - usersCountBefore;

            Assert.IsTrue(difference == 1);

            //cleanup
            await repository.DeleteUser(usersAfter[usersAfter.Count - 1]);

        }
    }
}