using FSL.Framework.Core.Address.Models;
using FSL.Framework.Core.Address.Repository;
using FSL.Framework.Core.Extensions;
using FSL.Framework.Core.Models;
using FSL.Framework.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.Framework.Api.Controllers
{
    [Route("api/address")]
    [ApiController]
    public sealed class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(
            IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet("{id}")]
        public async Task<BaseResult<Address>> GetAsync(
            string id)
        {
            var data = await _addressRepository.GetAddressAsync(id);

            return data.ToResult();
        }

        [HttpGet("")]
        public async Task<BaseResult<IEnumerable<Address>>> GetRangeAsync(
            string start,
            string end)
        {
            var data = await _addressRepository.GetAddressRangeAsync(
                start,
                end);

            return data.ToResult();
        }
    }
}
