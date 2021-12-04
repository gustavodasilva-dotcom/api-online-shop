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
    public class ClientesRepository : IClientesRepository
    {
        private readonly string _connectionString;

        public ClientesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<ClienteTable> Obter(string cpf)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" SELECT	    C.ClienteId,
		            			    C.PrimeiroNome,
		            			    C.NomeDoMeio,
		            			    C.Sobrenome,
		            			    C.Cpf,
		            			    IC.InformacoesContatoId,
		            			    IC.Telefone,
		            			    IC.Celular,
		            			    IC.Email,
		            			    E.EnderecoId,
		            			    E.Cep,
		            			    E.Logradouro,
		            			    E.Numero,
		            			    E.Complemento,
		            			    E.Bairro,
		            			    E.Localidade,
		            			    E.Uf,
		            			    E.Pais
		                FROM		Cliente				C
		                INNER JOIN	InformacoesContato	IC	ON C.InformacoesContatoId	= IC.InformacoesContatoId
		                INNER JOIN	Endereco			E	ON C.EnderecoId				= E.EnderecoId
		                WHERE		C.Cpf = @cpf
		                  AND		C.Excluido  = 0
		                  AND		E.Excluido  = 0
		                  AND		IC.Excluido = 0;";

                    #endregion SQL

                    return await db.QueryFirstOrDefaultAsync<ClienteTable>(query, new { cpf });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Inserir(InformacoesContato informacoesContato)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION

		                	BEGIN TRY

		                		INSERT INTO InformacoesContato
		                		VALUES
		                		(
		                			 '{informacoesContato.Telefone}'
		                			,'{informacoesContato.Celular}'
		                			,'{informacoesContato.Email}'
		                			,GETDATE()
		                			,0
		                		);

		                	END TRY

		                	BEGIN CATCH

		                		IF @@TRANCOUNT > 0
		                			ROLLBACK TRANSACTION;

		                	END CATCH;

		                IF @@TRANCOUNT > 0
		                	COMMIT TRANSACTION

                        DECLARE @InfoContatoID INT = @@IDENTITY;

                        SELECT @InfoContatoID;";

                    #endregion SQL

                    return await db.ExecuteScalarAsync<int>(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Inserir(Cliente cliente)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION;

		                	BEGIN TRY

		                		INSERT INTO Cliente
		                		VALUES
		                		(
		                			 '{cliente.PrimeiroNome}'
		                			,'{cliente.NomeDoMeio}'
		                			,'{cliente.Sobrenome}'
		                			,'{cliente.Cpf}'
		                			,{cliente.Endereco.Codigo}
		                			,{cliente.InformacoesContato.Codigo}
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

        public async Task Atualizar(InformacoesContato informacoesContato)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION;

		                	BEGIN TRY

		                		UPDATE	InformacoesContato
		                		SET
		                				Telefone				= @Telefone,
		                				Celular					= @Celular,
		                				Email					= @Email
		                		WHERE	InformacoesContatoId	= @Codigo

		                	END TRY

		                	BEGIN CATCH

		                		IF @@TRANCOUNT > 0
		                			ROLLBACK TRANSACTION;

		                	END CATCH;

		                IF @@TRANCOUNT > 0
		                	COMMIT TRANSACTION;";

                    #endregion SQL

                    await db.ExecuteAsync(query, new { informacoesContato.Telefone, informacoesContato.Celular, informacoesContato.Email, informacoesContato.Codigo });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Atualizar(Cliente cliente)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    @"  BEGIN TRANSACTION;

		            	BEGIN TRY

		            		UPDATE	Cliente
		            		SET
		            				PrimeiroNome	= @PrimeiroNome,
		            				NomeDoMeio		= @NomeDoMeio,
		            				Sobrenome		= @Sobrenome,
		            				Cpf				= @Cpf
		            		WHERE	ClienteId		= @Codigo;

		            	END TRY

		            	BEGIN CATCH

		            		IF @@TRANCOUNT > 0
		            			ROLLBACK TRANSACTION;

		            	END CATCH;

		            IF @@TRANCOUNT > 0
		            	COMMIT TRANSACTION;";

                    #endregion SQL

                    await db.ExecuteAsync(query, new { cliente.PrimeiroNome, cliente.NomeDoMeio, cliente.Sobrenome, cliente.Cpf, cliente.Codigo });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Deletar(ClienteTable cliente)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION;

		                	BEGIN TRY

		                		UPDATE	Cliente
		                		SET
		                				Excluido	= 1
		                		WHERE	ClienteId	= @ClienteId;

		                	END TRY

		                	BEGIN CATCH

		                		IF @@TRANCOUNT > 0
		                			ROLLBACK TRANSACTION;

		                	END CATCH;

		                IF @@TRANCOUNT > 0
		                	COMMIT TRANSACTION;";

                    #endregion SQL

                    await db.ExecuteAsync(query, new { cliente.ClienteId });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
