using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
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

        public async Task<int> Inserir(Produto produto)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" DECLARE @ProdutoId INT;

                        INSERT INTO Produto
                        VALUES
                        (
                        	 '{produto.Nome}'
                        	,'{produto.Medida}'
                        	,'{produto.Preco}'
                        	,{produto.Categoria.Codigo}
                        	,{produto.Fornecedor.Codigo}
                        	,GETDATE()
                        	,0
                        );
                        
                        SET @ProdutoId = @@IDENTITY;
                        
                        SELECT @ProdutoId;";

                    #endregion SQL

                    return await db.ExecuteScalarAsync<int>(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InserirBase64(string base64, int produtoId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" INSERT INTO ProdutoImagem
                        VALUES
                        (
                        	 '{base64}'
                        	,{produtoId}
                        	,GETDATE()
                        	,0
                        );";

                    await db.ExecuteAsync(query);

                    #endregion SQL
                }
            }
            catch (Exception)
            {
                throw;
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
