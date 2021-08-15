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
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly string _connectionString;

        public ProdutosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IEnumerable<ProdutoViewModel>> Obter(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var produtos = await db.QueryAsync<ProdutoViewModel>(query);

                return produtos;
            }
        }

        public async Task<ProdutoViewModel> ExecutarComando(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var produto = db.QuerySingleAsync<ProdutoViewModel>(query, new { ProdutoViewModel = 1 });

                return await produto;
            }
        }
    }
}
