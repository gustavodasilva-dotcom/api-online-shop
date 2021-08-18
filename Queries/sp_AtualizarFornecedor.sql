USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_AtualizarFornecedor]
	@CnpjRota		VARCHAR(255),
	@NomeFantasia	VARCHAR(255),
	@RazaoSocial	VARCHAR(255),
	@Cnpj			CHAR(14),
	@Contato		VARCHAR(255),
	@Cep			VARCHAR(255),
	@Logradouro		VARCHAR(255),
	@Numero			VARCHAR(255),
	@Complemento	VARCHAR(255),
	@Bairro			VARCHAR(255),
	@Localidade		VARCHAR(255),
	@Uf				CHAR(2),
	@Pais			VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para atualizar dados de fornecedores.
Data de criação: 13-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem		VARCHAR(255);
		DECLARE @FornecedorId	INT;
		DECLARE @EnderecoId		INT;
		DECLARE @InfoContatoId	INT;


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @CnpjRota) IS NULL
		BEGIN
			SET @Mensagem = 'O Cnpj ' + @CnpjRota + ' não consta em sistema.';
			PRINT @Mensagem;
			PRINT '';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @CnpjRota AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'O Cnpj ' + @CnpjRota + ' não consta em sistema.';
			PRINT @Mensagem;
			PRINT '';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		PRINT 'Selecionando os ids do Fornecedor e Endereco com base no CNPJ ' + @CnpjRota;
		PRINT '';

		SELECT @FornecedorId	= FornecedorId	FROM Fornecedor WHERE Cnpj = @CnpjRota AND Excluido = 0

		SELECT @EnderecoId		= EnderecoId	FROM Fornecedor WHERE Cnpj = @CnpjRota AND FornecedorId = @FornecedorId AND Excluido = 0


		IF EXISTS (SELECT 1 FROM Fornecedor WHERE FornecedorId = @FornecedorId AND Excluido = 0)
			BEGIN
				SET @Mensagem = 'Fornecedor encontrado em sistema: ' + CAST(@FornecedorId AS VARCHAR(8));
				
				PRINT @Mensagem;
				PRINT '';

				SET @Mensagem = NULL;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'Fornecedor não encontrado em sistema.';

				PRINT @Mensagem;
				PRINT '';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END


		IF EXISTS (SELECT 1 FROM Endereco WHERE EnderecoId = @EnderecoId AND Excluido = 0)
			BEGIN
				SET @Mensagem = 'Endereco encontrado em sistema: ' + CAST(@EnderecoId AS VARCHAR(8));
				
				PRINT @Mensagem;
				PRINT '';

				SET @Mensagem = NULL;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'Endereco não encontrado em sistema.';

				PRINT @Mensagem;
				PRINT '';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END

		
		BEGIN TRANSACTION

			UPDATE	Fornecedor
			SET
					NomeFantasia	= @NomeFantasia,
					RazaoSocial		= @RazaoSocial,
					Cnpj			= @Cnpj,
					Contato			= @Contato		
			WHERE	FornecedorId	= @FornecedorId

		COMMIT TRANSACTION


		SET @Mensagem = 'Fornecedor atualizado no banco de dados com sucesso!';
		PRINT @Mensagem;
		PRINT '';

		SET @Mensagem = NULL;


		BEGIN TRANSACTION

			UPDATE	Endereco
			SET
					Cep				= @Cep,
					Logradouro		= @Logradouro,
					Numero			= @Numero,
					Complemento		= @Complemento,
					Bairro			= @Bairro,
					Localidade		= @Localidade,
					Uf				= @Uf,
					Pais			= @Pais
			WHERE	EnderecoId		= @EnderecoId

		COMMIT TRANSACTION


		SET @Mensagem = 'Endereco atualizado no banco de dados com sucesso!';
		PRINT @Mensagem;
		PRINT '';

		SET @Mensagem = NULL;


		SELECT		NomeFantasia,
					RazaoSocial,
					Cnpj,
					Contato,
					Cep,
					Logradouro,
					Numero,
					Complemento,
					Bairro,
					Localidade,
					Uf,
					Pais
		FROM		Fornecedor	F
		INNER JOIN	Endereco	E ON F.EnderecoId = E.EnderecoId
		WHERE		FornecedorId = @FornecedorId;

	END
GO