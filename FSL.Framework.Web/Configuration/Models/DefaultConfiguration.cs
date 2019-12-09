using FSL.Framework.Core.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace FSL.Framework.Web.Configuration.Models
{
    public class DefaultConfiguration : IDefaultConfiguration
    {
        private readonly IConfiguration _configuration;

        public DefaultConfiguration(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionStringId { get; set; }

        public string GetConnectionString(
            string connectionStringId)
        {
            return _configuration.GetConnectionString(connectionStringId);
        }
    }
}
