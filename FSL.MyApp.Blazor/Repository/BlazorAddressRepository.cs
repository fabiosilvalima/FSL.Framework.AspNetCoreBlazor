using FSL.Framework.Core.Address.Models;
using FSL.Framework.Core.Address.Repository;
using FSL.Framework.Core.ApiClient.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.MyApp.Blazor.Repository
{
    public sealed class BlazorAddressRepository : IAddressRepository
    {
        private readonly IApiClientService _apiClientService;

        public BlazorAddressRepository(
            IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<Address> GetAddressAsync(
            string zipCode)
        {
            var apiClient = await _apiClientService.CreateInstanceAsync();
            var result = await apiClient.GetAsync<Address>($"address/{zipCode}");

            return result.Data;
        }

        public async Task<IEnumerable<Address>> GetAddressRangeAsync(
            string start, 
            string end)
        {
            var apiClient = await _apiClientService.CreateInstanceAsync();
            var result = await apiClient.GetAsync<List<Address>>($"address?start={start}&end={end}");

            return result.Data;
        }
    }
}
