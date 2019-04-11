
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Plugin.RestClient;
using Glimpse.Core.Services.General;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Glimpse.Core.Services.Data
{
    public class LoginDataService : ILoginDataService
    {
        private readonly IVendorRepository _vendorRepository;
        private Vendor vendor;

        public LoginDataService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<bool> AuthenticateVendor(string email, string enteredPassword)
        {
            Vendor vendor = new Vendor { Email = email, Password = enteredPassword };

            RestClient<Vendor> restclient = new RestClient<Vendor>();

            var response = await restclient.Authenticate(vendor);

             if (response)
             {
                 SaveEmailPasswordInSettings(email, enteredPassword);
                 return true;
             }
             else
             {
                 return false;
             }
        }

        public bool AuthenticateUserLogin()
        {
            return !string.IsNullOrEmpty(Settings.Email);
        }

        public void SaveEmailPasswordInSettings(string email, string hashedPassword)
        {
            Settings.Email = email;
            Settings.Password = hashedPassword;
            Settings.LoginStatus = true;
        }

        public void ClearCredentials()
        {
            Settings.Email = string.Empty;
            Settings.Password = string.Empty;
        }

        public void ClearLoginState()
        {
            Settings.LoginStatus = false;
        }

    }
}