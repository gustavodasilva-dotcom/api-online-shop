using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ApiOnlineShop.Repositories
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly string _connectionString;

        public ClientesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<ClienteViewModel> Obter(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var cliente = db.QueryFirstOrDefaultAsync<ClienteViewModel>(query);

                return await cliente;
            }
        }

        public async Task<ClienteViewModel> ExecutarComando(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var cliente = db.QuerySingleAsync<ClienteViewModel>(query, new { ClienteViewModel = 1 } );

                return await cliente;
            }
        }

    }
}
