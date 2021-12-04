using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Repositories
{
    public class InformacoesContatosRepository : IInformacoesContatosRepository
    {
        private readonly string _connectionString;

        public InformacoesContatosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task Deletar(int informacoesContatoId)
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
		                				Excluido	= 1
		                		WHERE	InformacoesContatoId = @InformacoesContatoId;

		                	END TRY

		                	BEGIN CATCH

		                		IF @@TRANCOUNT > 0
		                			ROLLBACK TRANSACTION;

		                	END CATCH;

		                IF @@TRANCOUNT > 0
		                	COMMIT TRANSACTION;";

                    #endregion SQL

                    await db.ExecuteAsync(query, new { informacoesContatoId });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
