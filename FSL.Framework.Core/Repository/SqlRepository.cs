using FSL.Framework.Core.Configuration.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FSL.Framework.Core.Repository
{
    public class SqlRepository
    {
        private string _connectionStringId;
        private readonly IDefaultConfiguration _configuration;

        public SqlRepository(
            IDefaultConfiguration configuration)
        {
            _configuration = configuration;
            UseConnectionStringId(configuration?.ConnectionStringId ?? "Default");
        }

        protected async Task<T> WithConnectionAsync<T>(
            Func<SqlConnection, Task<T>> getData)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();

                var data = await getData(connection);

                connection.Close();

                return data;
            };
        }
        protected SqlRepository UseConnectionStringId(
            string connectionStringId)
        {
            _connectionStringId = connectionStringId;

            return this;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString(_connectionStringId));
        }
    }
}
