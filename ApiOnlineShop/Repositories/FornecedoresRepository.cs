using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories
{
    public class FornecedoresRepository : IFornecedoresRepository
    {
        private readonly string _connectionString;

        public FornecedoresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<FornecedorViewModel> Obter(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var fornecedor = db.QueryFirstOrDefaultAsync<FornecedorViewModel>(query);

                return await fornecedor;
            }
        }

        public async Task<FornecedorViewModel> ExecutarComando(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var fornecedor = db.QuerySingleAsync<FornecedorViewModel>(query, new { FornecedorViewModel = 1 });

                return await fornecedor;
            }
        }

    }
}
