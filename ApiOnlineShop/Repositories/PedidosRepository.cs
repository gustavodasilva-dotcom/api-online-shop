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
    public class PedidosRepository : IPedidosRepository
    {
        private readonly string _connectionString;

        public PedidosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<PedidoTable> Obter(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);
                
                #region SQL

                var query =
                $@" SELECT	 P.PedidoId
                			,C.ClienteId
                			,CONCAT(C.PrimeiroNome, ' ', C.NomeDoMeio, ' ', C.Sobrenome) AS Cliente
                			,C.CPF
                			,E.Cep
                			,E.Logradouro
                			,E.Numero
                			,CASE 
                				WHEN E.Complemento IS NULL
                					THEN ''
                				ELSE E.Complemento
                			 END AS Complemento
                			,E.Bairro
                			,E.Localidade
                			,E.Uf
                			,E.Pais
                			,DataCompra
                FROM		Pedido		P (NOLOCK)
                INNER JOIN	Cliente		C (NOLOCK) ON P.ClienteId	= C.ClienteId
                INNER JOIN	Endereco	E (NOLOCK) ON C.EnderecoId	= E.EnderecoId
                WHERE		PedidoId = {id};";

                #endregion SQL

                return await db.QuerySingleOrDefaultAsync<PedidoTable>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ItemsTable>> ObterItems(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(_connectionString);

                #region SQL

                var query =
                $@"  SELECT		 P.PedidoId
                    			,DP.ProdutoId
                    			,Quantidade
                    			,PR.Nome
                    			,PR.Medida
                    			,CAST(PR.Preco AS VARCHAR(20)) AS Preco
                    			,CASE
                    				WHEN (F.NomeFantasia IS	NULL) OR (F.NomeFantasia = '') THEN F.RazaoSocial
                    				ELSE F.NomeFantasia
                    			END AS Fornecedor
                    FROM		Pedido			P	(NOLOCK)
                    INNER JOIN	DetalhesPedido	DP	(NOLOCK) on P.PedidoId = DP.PedidoId
                    INNER JOIN	Produto			PR	(NOLOCK) on DP.ProdutoId = PR.ProdutoId
                    INNER JOIN	Fornecedor		F	(NOLOCK) on PR.FornecedorId = F.FornecedorId
                    WHERE		P.PedidoId = {id}
                    AND			P.Excluido = 0
                    AND			DP.Excluido = 0";

                #endregion SQL

                return await db.QueryAsync<ItemsTable>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InserirPedido(Pedido pedido)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" DECLARE @PedidoID INT;

                        INSERT INTO Pedido
                        VALUES
                        (
                        	 '{pedido.DataCompra}'
                        	,{pedido.Cliente.Codigo}
                        	,GETDATE()
                        	,0
                        );
                        
                        SET @PedidoID = @@IDENTITY;
                        
                        SELECT @PedidoID;";

                    #endregion SQL

                    return await db.ExecuteScalarAsync<int>(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InserirDetalhesPedido(Pedido pedido, int pedidoID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    foreach (var detalhe in pedido.DetalhesPedido)
                    {
                        var query =
                        $@" INSERT INTO DetalhesPedido
                        VALUES
                        (
                        	 {detalhe.Quantidade}
                        	,{detalhe.Produto.Codigo}
                        	,{pedidoID}
                        	,GETDATE()
                        	,0
                        );";

                        await db.ExecuteAsync(query);
                    }

                    #endregion SQL
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoViewModel> ExecutarComando(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var pedido = await db.QuerySingleAsync<PedidoViewModel>(query, new { PedidoViewModel = 1 });

                return pedido;
            }
        }
    }
}
