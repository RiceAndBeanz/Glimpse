using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Model;

namespace Glimpse.Core.Contracts.Services
{
    public interface IUserDataService
    {
        Task<User> SearchUserByEmail(string email);

        Task SignUp(User user);

        Task<List<User>> GetUsers();

        Task DeleteUser(User user);

    }
}