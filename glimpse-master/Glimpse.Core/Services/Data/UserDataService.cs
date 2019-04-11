
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Plugin.RestClient;
using Glimpse.Core.Services.General;
using System.Collections.Generic;

namespace Glimpse.Core.Services.Data
{
    public class UserDataService: IUserDataService
    {
        private readonly IUserRepository _userRepository;

        public UserDataService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<User> SearchUserByEmail(string email)
        {
            return await _userRepository.SearchUserByEmail(email);
        }

        public async Task SignUp(User user)
        {
            var cryptoTuple = Cryptography.HashPassword(user.Password);
            user.Password = cryptoTuple.Item1;
            user.Salt = cryptoTuple.Item2;
            await _userRepository.PostUser(user);
        }

        public async Task DeleteUser(User user)
        {
           await _userRepository.DeleteUser(user);
        }

    }
}