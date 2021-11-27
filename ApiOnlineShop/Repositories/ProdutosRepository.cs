using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Entities.Table;
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

        public async Task<IEnumerable<ProdutoTable>> Obter(int categoriaId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" SELECT		 P.ProdutoId
                        			,P.Nome
                        			,P.Medida
                        			,P.Preco
                        			,PI.Base64
                                    ,C.CategoriaId
                        			,C.Nome			AS 'Categoria'
                        			,C.Descricao	AS 'Descricao'
                        			,F.RazaoSocial	AS 'Fornecedor'
                        			,F.Cnpj			AS 'Cnpj'  
                        FROM		Produto			P
                        INNER JOIN	Fornecedor		F	ON P.FornecedorId = F.FornecedorId
                        INNER JOIN	Categoria		C	ON P.CategoriaId  = C.CategoriaId
                        LEFT  JOIN	ProdutoImagem	PI	ON P.ProdutoId	  = PI.ProdutoId
                        WHERE		C.CategoriaId = {categoriaId}
                         AND		C.Excluido = 0
                         AND		P.Excluido = 0
                         AND		F.Excluido = 0;;";

                    #endregion SQL

                    return await db.QueryAsync<ProdutoTable>(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoTable> Obter(int produtoId, int categoriaId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@"SELECT		 P.ProdutoId
                        			,P.Nome
                        			,P.Medida
                        			,P.Preco
                        			,PI.Base64
                        			,C.CategoriaId
                        			,C.Nome			AS 'Categoria'
                        			,C.Descricao	AS 'Descricao'
                        			,F.RazaoSocial	AS 'Fornecedor'
                        			,F.Cnpj			AS 'Cnpj'  
                        FROM		Produto			P
                        INNER JOIN	Fornecedor		F	ON P.FornecedorId = F.FornecedorId
                        INNER JOIN	Categoria		C	ON P.CategoriaId  = C.CategoriaId
                        LEFT  JOIN	ProdutoImagem	PI	ON P.ProdutoId	  = PI.ProdutoId
                        WHERE		P.ProdutoId   = {produtoId}
                         AND        C.CategoriaId = {categoriaId}
                         AND		C.Excluido    = 0
                         AND		P.Excluido    = 0
                         AND		F.Excluido    = 0;";

                    #endregion SQL

                    return await db.QueryFirstOrDefaultAsync<ProdutoTable>(query, new { produtoId });
                }
            }
            catch (Exception)
            {
                throw;
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
