using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories
{
    public class PedidosRepository : IPedidosRepository
    {
        private readonly string _connectionString;

        public PedidosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<PedidoViewModel> Obter(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var pedido = await db.QuerySingleOrDefaultAsync<PedidoViewModel>(query);

                return pedido;
            }
        }

    }
}
