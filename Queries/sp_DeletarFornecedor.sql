USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_DeletarFornecedor]
	@Cnpj			VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para deletar fornecedores.
Data de criação: 13-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT OFF;

		DECLARE @Mensagem		VARCHAR(255);
		DECLARE @FornecedorId	INT;
		DECLARE @EnderecoId		INT;


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj) IS NULL
		BEGIN
			SET @Mensagem = 'Fornecedor não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 16, 1);
		END


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'Fornecedor não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 16, 1);
		END


		SELECT @FornecedorId = FornecedorId FROM Fornecedor WHERE Cnpj = @Cnpj AND Excluido = 0;

		IF @FornecedorId IS NOT NULL
		BEGIN
			SET @Mensagem = 'Fornecedor encontrado no sistema: ' + CAST(@FornecedorId AS VARCHAR(8));
			PRINT @Mensagem;
		END

		SET @Mensagem = NULL;


		SELECT	@EnderecoId		= EnderecoId
		FROM	Fornecedor
		WHERE	FornecedorId	= @FornecedorId;


		IF (SELECT 1 FROM Endereco WHERE EnderecoId = @EnderecoId) IS NULL
			BEGIN
				SET @Mensagem = 'Endereco não consta em sistema.';
				PRINT @Mensagem;

				RAISERROR(@Mensagem, 16, 1);
			END

		IF (SELECT 1 FROM Endereco WHERE EnderecoId = @EnderecoId AND Excluido = 1) IS NOT NULL
			BEGIN
				SET @Mensagem = 'Endereco não consta em sistema.';
				PRINT @Mensagem;

				RAISERROR(@Mensagem, 16, 1);
			END
		ELSE
			BEGIN
				SET @Mensagem = 'Endereco encontrado no sistema: ' + CAST(@EnderecoId AS VARCHAR(8));
				PRINT @Mensagem;

				SET @Mensagem = NULL;
			END


		BEGIN TRANSACTION

			UPDATE	Fornecedor
			SET
					Excluido		= 1
			WHERE	FornecedorId	= @FornecedorId;

		COMMIT TRANSACTION

		SET @Mensagem = 'Fornecedor deletado com sucesso!';
		SET @Mensagem = NULL;


		BEGIN TRANSACTION

			UPDATE	Endereco
			SET
					Excluido	= 1
			WHERE	EnderecoId	= @EnderecoId;

			SET @Mensagem = 'Endereco deletado com sucesso!';
			SET @Mensagem = NULL;

		COMMIT TRANSACTION

	END
GO