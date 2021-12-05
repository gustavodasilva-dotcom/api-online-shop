using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly string _connectionString;

        public LogsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task GravarLog(string mensagem, object entrada, object retorno, int id, bool email)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION;

                        	BEGIN TRY
                        
                        		INSERT INTO LOGS
                        		VALUES
                        		(
                        			 @mensagem
                        			,@entrada
                        			,@retorno
                        			,@id
                        			,@email
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

                    await db.ExecuteAsync(query, new { mensagem, entrada, retorno, id, email });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GravarLog(string mensagem, object entrada, bool email)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    #region SQL

                    var query =
                    $@" BEGIN TRANSACTION;

                        	BEGIN TRY
                        
                        		INSERT INTO LOGS
                        		(
                        			 Mensagem
                        			,JsonEntrada
                        			,RetornoEmail
                        			,DataInsercao
                        			,Excluido
                        		)
                        		VALUES
                        		(
                        			 @mensagem
                        			,@entrada
                        			,@email
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

                    await db.ExecuteAsync(query, new { mensagem, entrada, email });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
