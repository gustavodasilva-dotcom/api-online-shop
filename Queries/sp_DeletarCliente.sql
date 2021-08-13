USE OnlineShop
GO

CREATE PROCEDURE [dbo].[sp_DeletarCliente]
	@Cpf			VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para deletar clientes.
Data de criação: 12-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT OFF;

		DECLARE @Mensagem		VARCHAR(255);
		DECLARE @ClienteId		INT;
		DECLARE @EnderecoId		INT;
		DECLARE @InfoContatoId	INT;


		IF (SELECT 1 FROM Cliente WHERE Cpf = @Cpf) IS NULL
		BEGIN
			SET @Mensagem = 'Cliente não consta em sistema.';
			PRINT @Mensagem;

			RETURN;
		END


		IF (SELECT 1 FROM Cliente WHERE Cpf = @Cpf AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'Cliente não consta em sistema.';
			PRINT @Mensagem;

			RETURN;
		END


		SELECT @ClienteId = ClienteId FROM Cliente WHERE Cpf = @Cpf;

		IF @ClienteId IS NOT NULL
		BEGIN
			SET @Mensagem = 'Cliente encontrado no sistema: ' + CAST(@ClienteId AS VARCHAR(8));
			PRINT @Mensagem;
		END

		SET @Mensagem = NULL;


		SELECT	@EnderecoId		= EnderecoId,
				@InfoContatoId	= InformacoesContatoId
		FROM	Cliente
		WHERE	ClienteId		= @ClienteId;


		IF (SELECT 1 FROM Endereco WHERE EnderecoId = @EnderecoId) IS NULL
			BEGIN
				SET @Mensagem = 'Endereco não consta em sistema.';
				PRINT @Mensagem;

				RETURN;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'Endereco encontrado no sistema: ' + CAST(@EnderecoId AS VARCHAR(8));
				PRINT @Mensagem;

				SET @Mensagem = NULL;
			END


		IF (SELECT 1 FROM InformacoesContato WHERE InformacoesContatoId = @InfoContatoId) IS NULL
			BEGIN
				SET @Mensagem = 'InformacoesContato não consta em sistema.';
				PRINT @Mensagem;

				RETURN;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'InformacoesContato encontrado no sistema: ' + CAST(@InfoContatoId AS VARCHAR(8));
				PRINT @Mensagem;

				SET @Mensagem = NULL;
			END


		BEGIN TRANSACTION

			UPDATE	Cliente
			SET
					Excluido	= 1
			WHERE	ClienteId	= @ClienteId;

			SET @Mensagem = 'Cliente deletado com sucesso!';
			SET @Mensagem = NULL;

			UPDATE	Endereco
			SET
					Excluido	= 1
			WHERE	EnderecoId	= @EnderecoId;

			SET @Mensagem = 'Endereco deletado com sucesso!';
			SET @Mensagem = NULL;


			UPDATE	InformacoesContato
			SET
					Excluido	= 1
			WHERE	InformacoesContatoId = @InfoContatoId;

			SET @Mensagem = 'InformacoesContato deletado com sucesso!';
			SET @Mensagem = NULL;

		COMMIT TRANSACTION

	END
GO