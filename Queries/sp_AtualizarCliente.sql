USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_AtualizarCliente]
	@CpfRota		VARCHAR(255),
	@PrimeiroNome	VARCHAR(255),
	@NomeDoMeio		VARCHAR(255),
	@Sobrenome		VARCHAR(255),
	@Cpf			VARCHAR(255),
	@Telefone		VARCHAR(15),
	@Celular		VARCHAR(15),
	@Email			VARCHAR(255),
	@Cep			VARCHAR(255),
	@Logradouro		VARCHAR(255),
	@Numero			VARCHAR(255),
	@Complemento 	VARCHAR(255),
	@Bairro			VARCHAR(255),
	@Localidade		VARCHAR(255), 
	@Uf				CHAR(2),
	@Pais 			VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descri??o......: Procedure utilizada para atualizar dados de clientes.
Data de cria??o: 11-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem		VARCHAR(255);
		DECLARE @ClienteId		INT;
		DECLARE @EnderecoId		INT;
		DECLARE @InfoContatoId	INT;


		IF (SELECT 1 FROM Cliente WHERE Cpf = @CpfRota) IS NULL
		BEGIN
			SET @Mensagem = 'O Cpf ' + @CpfRota + ' n?o consta em sistema.';
			PRINT @Mensagem;
			PRINT '';

			RAISERROR(@Mensagem, 20, -1) WITH LOG; 
		END

		IF (SELECT 1 FROM Cliente WHERE Cpf = @CpfRota AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'O Cpf ' + @CpfRota + ' n?o consta em sistema.';
			PRINT @Mensagem;
			PRINT '';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		PRINT 'Selecionando os ids do Cliente, Endereco e InformacoesContato com base no CPF ' + @CpfRota;
		PRINT '';

		SELECT @ClienteId		= ClienteId				FROM Cliente WHERE Cpf = @CpfRota AND Excluido = 0

		SELECT @EnderecoId		= EnderecoId			FROM Cliente WHERE Cpf = @CpfRota AND ClienteId = @ClienteId AND Excluido = 0

		SELECT @InfoContatoId	= InformacoesContatoId	FROM Cliente WHERE Cpf = @CpfRota AND ClienteId = @ClienteId AND Excluido = 0


		IF EXISTS (SELECT 1 FROM Cliente WHERE ClienteId = @ClienteId AND Excluido = 0)
			BEGIN
				SET @Mensagem = 'Cliente encontrado em sistema: ' + CAST(@ClienteId AS VARCHAR(8));
				
				PRINT @Mensagem;
				PRINT '';

				SET @Mensagem = NULL;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'Cliente n?o encontrado em sistema.';

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
				SET @Mensagem = 'Endereco n?o encontrado em sistema.';

				PRINT @Mensagem;
				PRINT '';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END


		IF EXISTS (SELECT 1 FROM InformacoesContato WHERE InformacoesContatoId = @InfoContatoId AND Excluido = 0)
			BEGIN
				SET @Mensagem = 'InformacoesContato encontrado em sistema: ' + CAST(@InfoContatoId AS VARCHAR(8));
				
				PRINT @Mensagem;
				PRINT '';

				SET @Mensagem = NULL;
			END
		ELSE
			BEGIN
				SET @Mensagem = 'InformacoesContato n?o encontrado em sistema.';

				PRINT @Mensagem;
				PRINT '';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END

		
		BEGIN TRANSACTION

			UPDATE	Cliente
			SET
					PrimeiroNome	= @PrimeiroNome,
					NomeDoMeio		= @NomeDoMeio,
					Sobrenome		= @Sobrenome,
					Cpf				= @Cpf
			WHERE	ClienteId		= @ClienteId

		COMMIT TRANSACTION

		SET @Mensagem = 'Cliente atualizado no banco de dados com sucesso!';
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


		BEGIN TRANSACTION

			UPDATE	InformacoesContato
			SET
					Telefone				= @Telefone,
					Celular					= @Celular,
					Email					= @Email
			WHERE	InformacoesContatoId	= @InfoContatoId

		COMMIT TRANSACTION

		SET @Mensagem = 'InformacoesContato atualizado no banco de dados com sucesso!';
		PRINT @Mensagem;
		PRINT '';

		SET @Mensagem = NULL;


		SELECT		C.PrimeiroNome,
					C.NomeDoMeio,
					C.Sobrenome,
					C.Cpf,
					IC.Telefone,
					IC.Celular,
					IC.Email,
					E.Cep,
					E.Logradouro,
					E.Numero,
					E.Complemento,
					E.Bairro,
					E.Localidade,
					E.Uf,
					E.Pais
		FROM		Cliente				C
		INNER JOIN	InformacoesContato	IC  ON C.InformacoesContatoId = IC.InformacoesContatoId
		INNER JOIN	Endereco			E	ON C.EnderecoId = E.EnderecoId
		WHERE		C.ClienteId = @ClienteId;

	END
GO