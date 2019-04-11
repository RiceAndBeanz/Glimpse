using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Model;

namespace Glimpse.Core.Contracts.Services
{
    public interface ILoginDataService
    {
        Task<bool> AuthenticateVendor(string email, string password);
        //bool AuthenticateUser(User user, string email, string password);
        bool AuthenticateUserLogin();
        void SaveEmailPasswordInSettings(string email, string hashedPassword);
        void ClearCredentials();
        void ClearLoginState();
    }
}