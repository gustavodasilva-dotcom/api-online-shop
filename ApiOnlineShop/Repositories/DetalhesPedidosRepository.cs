using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories
{
    public class DetalhesPedidosRepository : IDetalhesPedidosRepository
    {
        private readonly string _connectionString;

        public DetalhesPedidosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IEnumerable<DetalhesPedidoViewModel>> Obter(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var produtos = await db.QueryAsync<DetalhesPedidoViewModel>(query);

                return produtos;
            }
        }
    }
}
