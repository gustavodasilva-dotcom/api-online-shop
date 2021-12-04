USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_CadastrarCliente]
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
Descrição......: Procedure utilizada para cadastrar clientes.
Data de criação: 10-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @IdEndereco		INT;
		DECLARE @IdInfoContato	INT;
		DECLARE @Mensagem		VARCHAR(255);


		IF EXISTS (SELECT 1 FROM Cliente WHERE Cpf = @Cpf)
		BEGIN
			SET @Mensagem = 'O CPF ' + @Cpf + ' já consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		PRINT 'Inserindo na tabela Endereco: ';

		BEGIN TRANSACTION

			INSERT INTO Endereco
			VALUES
			(
				@Cep,
				@Logradouro,
				@Numero,
				@Complemento,
				@Bairro,
				@Localidade,
				@Uf,
				@Pais,
				GETDATE(),
				0
			);

		COMMIT TRANSACTION

		PRINT 'Inseriu na tabela de Endereco.';

		
		SELECT @IdEndereco = EnderecoId FROM Endereco ORDER BY DataInsercao;

		PRINT 'Id de Endereco cadastrado: ' + CAST(@IdEndereco AS VARCHAR(255));


		PRINT 'Inserindo na tabela de InformacoesContato: ';


		BEGIN TRANSACTION

			BEGIN TRY

				INSERT INTO InformacoesContato
				VALUES
				(
					@Telefone,
					@Celular,
					@Email,
					GETDATE(),
					0
				);

			END TRY

			BEGIN CATCH

				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;

			END CATCH;

		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION

		PRINT 'Inseriu na tabela de InformacoesContato.';


		PRINT 'Inserindo na tabela de Cliente: ';
		
		SELECT @IdInfoContato = InformacoesContatoId FROM InformacoesContato ORDER BY DataInsercao;

		PRINT 'Id de InformacoesContato cadastrado: ' + CAST(@IdInfoContato AS VARCHAR(255));


		BEGIN TRANSACTION;

			BEGIN TRY

				INSERT INTO Cliente
				VALUES
				(
					@PrimeiroNome,
					@NomeDoMeio,
					@Sobrenome,
					@Cpf,
					@IdEndereco,
					@IdInfoContato,
					GETDATE(),
					0
				);

			END TRY

			BEGIN CATCH

				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;

			END CATCH;

		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION;

		PRINT 'Inseriu na tabela de Cliente.';


		SELECT		TOP 1
					C.PrimeiroNome,
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
		ORDER BY	C.ClienteId DESC;

	END
GO