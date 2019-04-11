using System.Threading.Tasks;
using Glimpse.Core.Model;
using System.Collections.Generic;

namespace Glimpse.Core.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<User> SearchUserByEmail(string email);

        Task DeleteUser(User user);

        Task PostUser(User user);

        Task<List<User>> GetUsers();
    }
}