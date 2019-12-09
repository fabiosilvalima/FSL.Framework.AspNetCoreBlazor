using Dapper;
using FSL.Framework.Core.Address.Models;
using FSL.Framework.Core.Address.Repository;
using FSL.Framework.Core.Repository;
using FSL.Framework.Web.Configuration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.MyApp.Api.Repository
{
    public sealed class ApiAddressSqlRepository : 
        SqlRepository,
        IAddressRepository
    {
        public ApiAddressSqlRepository(
            DefaultConfiguration configuration) : base(configuration)
        {

        }

        public async Task<Address> GetAddressAsync(
            string zipCode)
        {
            return await WithConnectionAsync(async connection =>
            {
                var parameters = new
                {
                    zipCode
                };

                var sql = @"SELECT              a.cod_postal AS ZipCode,
                                                b.des_cidade AS City,
                                                c.des_sigla AS State,
                                                (t.des_tipo_logradouro + ' ' + d.des_logradouro) AS Street, 
                                                r.des_bairro AS Neighborhood
                            FROM                dbo.tb_cep AS a
                            LEFT OUTER JOIN     dbo.tb_cidade AS b ON a.cod_cidade = b.cod_cidade
                            LEFT OUTER JOIN     dbo.tb_estado AS c ON a.cod_estado = c.cod_estado
                            LEFT OUTER JOIN     dbo.tb_logradouro AS d ON a.cod_logradouro = d.cod_logradouro
                            LEFT OUTER JOIN     dbo.tb_tipo_logradouro AS t ON a.cod_tipo_logradouro = t.cod_tipo_logradouro
                            LEFT OUTER JOIN     dbo.tb_bairro AS r ON a.cod_bairro = r.cod_bairro
                            WHERE               a.cod_postal = @zipCode";

                return await connection.QueryFirstOrDefaultAsync<Address>(
                    sql,
                    parameters);
            });
        }

        public async Task<IEnumerable<Address>> GetAddressRangeAsync(
            string start, 
            string end)
        {
            return await WithConnectionAsync(async connection =>
            {
                var parameters = new
                {
                    start,
                    end
                };

                var sql = @"SELECT              a.cod_postal AS ZipCode,
                                                b.des_cidade AS City,
                                                c.des_sigla AS State,
                                                (t.des_tipo_logradouro + ' ' + d.des_logradouro) AS Street, 
                                                r.des_bairro AS Neighborhood
                            FROM                dbo.tb_cep AS a
                            LEFT OUTER JOIN     dbo.tb_cidade AS b ON a.cod_cidade = b.cod_cidade
                            LEFT OUTER JOIN     dbo.tb_estado AS c ON a.cod_estado = c.cod_estado
                            LEFT OUTER JOIN     dbo.tb_logradouro AS d ON a.cod_logradouro = d.cod_logradouro
                            LEFT OUTER JOIN     dbo.tb_tipo_logradouro AS t ON a.cod_tipo_logradouro = t.cod_tipo_logradouro
                            LEFT OUTER JOIN     dbo.tb_bairro AS r ON a.cod_bairro = r.cod_bairro
                            WHERE               a.cod_postal BETWEEN @start AND @end";

                return await connection.QueryAsync<Address>(
                    sql,
                    parameters);
            });
        }
    }
}
