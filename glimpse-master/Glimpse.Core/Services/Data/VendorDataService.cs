using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Glimpse.Core.Services.General;
using Glimpse.Core.Utility;


namespace Glimpse.Core.Services.Data
{
    public class VendorDataService : IVendorDataService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorDataService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<Vendor> SearchVendorByEmail(string email)
        {
            return await _vendorRepository.SearchVendorByEmail(email);
        }

        public async Task<bool> SignUp(Vendor vendor)
        {
            var cryptoTuple = Cryptography.HashPassword(vendor.Password);
            vendor.Password = cryptoTuple.Item1;
            vendor.Salt = cryptoTuple.Item2;
            return await _vendorRepository.PostVendor(vendor);
        }

        public async Task<List<Vendor>> GetVendors()
        {
            return await _vendorRepository.GetVendors();
        }

        public async Task<int> GetVendorId(string email)
        {
            return await _vendorRepository.GetVendorId(email);
        }

        public async Task EditVendor(int id, Vendor vendor)
        {         
            await _vendorRepository.PutVendor(id, vendor);
        }

        public async Task DeleteVendor(Vendor vendor)
        {
            await _vendorRepository.DeleteVendor(vendor);
        }

        public async Task<bool> CheckIfVendorExists(string email)
        {
            Vendor vendor = null;
            if (!string.IsNullOrEmpty(email))
            {
                 vendor = await SearchVendorByEmail(email);
            }

            if (vendor != null)
                return true;

            return false;
        }
    }
}