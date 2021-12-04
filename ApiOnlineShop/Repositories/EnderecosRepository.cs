using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApiOnlineShop.Entities.Entities;
using Microsoft.Extensions.Configuration;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Repositories
{
    public class EnderecosRepository : IEnderecosRepository
    {
		private readonly string _connectionString;

		public EnderecosRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("Default");
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

		public async Task Atualizar(Endereco endereco)
		{
			try
			{
				using (IDbConnection db = new SqlConnection(_connectionString))
				{
					#region SQL

					var query =
					$@" BEGIN TRANSACTION;

							BEGIN TRY

								UPDATE	Endereco
								SET
										 Cep			= '{endereco.Cep}'
										,Logradouro		= '{endereco.Logradouro}'
										,Numero			= '{endereco.Numero}'
										,Complemento	= '{endereco.Complemento}'
										,Bairro			= '{endereco.Bairro}'
										,Localidade		= '{endereco.Localidade}'
										,Uf				= '{endereco.Uf}'
										,Pais			= '{endereco.Pais}'
								WHERE	EnderecoId		= {endereco.Codigo}

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

		public async Task Deletar(int enderecoId)
        {
			try
            {
				using (IDbConnection db = new SqlConnection(_connectionString))
                {
					#region SQL

					var query =
					$@"	BEGIN TRANSACTION;

						BEGIN TRY

							UPDATE	Endereco
							SET
									Excluido	= 1
							WHERE	EnderecoId	= {enderecoId};

						END TRY

						BEGIN CATCH

							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;

						END CATCH;

					IF @@TRANCOUNT > 0
						COMMIT TRANSACTION;";

					#endregion SQL

					await db.ExecuteAsync(query, new { enderecoId });
				}
            }
			catch (Exception)
            {
				throw;
            }
		}
	}
}
