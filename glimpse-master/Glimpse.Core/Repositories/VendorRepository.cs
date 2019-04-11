using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Contracts.Repository;
using Glimpse.Core.Model;
using Plugin.RestClient;

namespace Glimpse.Core.Repositories
{
    public class VendorRepository : IVendorRepository
    {
 
        public async Task<Vendor> SearchVendorByEmail(string email)
        {
            RestClient<Vendor> restClient = new RestClient<Vendor>();
            return await restClient.GetByKeyword(email, true);
        }

        public async Task<bool> PostVendor(Vendor vendor)
        {
            RestClient<Vendor> restClient = new RestClient<Vendor>();

            return await restClient.PostAsync(vendor);
        }

        public async Task<List<Vendor>> GetVendors()
        {
            RestClient<Vendor> restClient = new RestClient<Vendor>();

            var vendorsList = await restClient.GetAsync();

            return vendorsList;
        }

        public async Task<int> GetVendorId(string email)
        {
            RestClient<int> restClient = new RestClient<int>();

            var vendorId = await restClient.GetIdAsync(email);

            return vendorId;
        }

        public async Task PutVendor(int id, Vendor vendor)
        {
            RestClient<Vendor> restClient = new RestClient<Vendor>();

            await restClient.PutAsync(id, vendor);
        }

        public async Task DeleteVendor(Vendor vendor)
        {
            RestClient<Vendor> restClient = new RestClient<Vendor>();

            await restClient.DeleteAsync(vendor.VendorId, vendor);
        }

    }
}
