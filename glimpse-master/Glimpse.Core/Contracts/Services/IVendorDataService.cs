using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Model;

namespace Glimpse.Core.Contracts.Services
{
    public interface IVendorDataService
    {
        Task<Vendor> SearchVendorByEmail(string email);

        Task<bool> SignUp(Vendor vendor);

        Task EditVendor(int id, Vendor vendor);

        Task<int> GetVendorId(string email);

        Task<List<Vendor>> GetVendors();

        Task DeleteVendor(Vendor vendor);

        Task<bool> CheckIfVendorExists(string email);
    }
}