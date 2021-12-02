using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Entities.Entities;
using Microsoft.Extensions.Configuration;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Repositories
{
    public class FornecedoresRepository : IFornecedoresRepository
    {
        private readonly string _connectionString;

        public FornecedoresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<FornecedorTable> Obter(string cnpj)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                #region SQL

                var query =
                @"SELECT		 NomeFantasia
		            			,RazaoSocial
		            			,Cnpj
		            			,Contato
		            			,Cep
		            			,Logradouro
		            			,Numero
		            			,Complemento
		            			,Bairro
		            			,Localidade
		            			,Uf
		            			,Pais
		            FROM		Fornecedor	F
		            INNER JOIN	Endereco	E ON F.EnderecoId = E.EnderecoId
		            WHERE		Cnpj = @cnpj
                      AND       F.Excluido = 0
                      AND       E.Excluido = 0";

                #endregion SQL

                return await db.QueryFirstOrDefaultAsync<FornecedorTable>(query, new { cnpj });
            }
        }

        public async Task<int> Inserir(Endereco endereco)
        {
			try
            {
				using (IDbConnection db = new SqlConnection(_connectionString))
				{
					#region SQL

					var query =
					$@"	DECLARE @EnderecoID INT;

						BEGIN TRANSACTION;

							BEGIN TRY

								INSERT INTO Endereco
								VALUES
								(
									 '{endereco.Cep}'
									,'{endereco.Logradouro}'
									,'{endereco.Numero}'
									,'{endereco.Complemento}'
									,'{endereco.Bairro}'
									,'{endereco.Localidade}'
									,'{endereco.Uf}'
									,'{endereco.Pais}'
									,GETDATE()
									,0
								);

							END TRY

							BEGIN CATCH

								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;

							END CATCH;

						IF @@TRANCOUNT > 0
							COMMIT TRANSACTION;

						SET @EnderecoID = @@IDENTITY;

						SELECT @EnderecoID;";

					#endregion SQL

					return await db.ExecuteScalarAsync<int>(query);
				}
			}
            catch (Exception)
            {
                throw;
            }
        }

		public async Task Inserir(Fornecedor fornecedor, int enderecoID)
        {
			try
            {
				using (IDbConnection db = new SqlConnection(_connectionString))
                {
					#region SQL

					var query =
					$@" BEGIN TRANSACTION;

							BEGIN TRY

								INSERT INTO Fornecedor
								VALUES
								(
									 '{fornecedor.NomeFantasia}'
									,'{fornecedor.RazaoSocial}'
									,'{fornecedor.Cnpj}'
									,'{fornecedor.Contato}'
									,{enderecoID}
									,GETDATE()
									,0
								);

							END TRY

							BEGIN CATCH

								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;

							END CATCH;

						IF @@TRANCOUNT > 0
							COMMIT TRANSACTION;";

					#endregion SQL

					await db.ExecuteAsync(query);
				}
			}
			catch (Exception)
            {
				throw;
            }
        }

        public async Task ExecutarComandoSemRetorno(string query)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(query);
            }
        }

    }
}
