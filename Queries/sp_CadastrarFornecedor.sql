USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_CadastrarFornecedor]
	@NomeFantasia	VARCHAR(255),
	@RazaoSocial	VARCHAR(255),
	@Cnpj			CHAR(14),
	@Contato		VARCHAR(255),
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
Descrição......: Procedure utilizada para cadastrar fornecedores.
Data de criação: 12-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT OFF;
		
		DECLARE @EnderecoId	INT;
		DECLARE @Mensagem	VARCHAR(255);

		IF EXISTS (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj)
		BEGIN
			SET @Mensagem = 'O CNPJ ' + @Cnpj + ' já consta em sistema.';
			PRINT @Mensagem;

			RETURN;
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
			)
		
		COMMIT TRANSACTION

		PRINT 'Inseriu na tabela de Endereco.';


		SELECT @EnderecoId = EnderecoId FROM Endereco ORDER BY DataInsercao;
		
		PRINT 'Id de Endereco cadastrado: ' + CAST(@EnderecoId AS VARCHAR(255));


		PRINT 'Inserindo na tabela de Fornecedor: ';

		BEGIN TRANSACTION

			INSERT INTO Fornecedor
			VALUES
			(
				@NomeFantasia,
				@RazaoSocial,
				@Cnpj,
				@Contato,
				@EnderecoId,
				GETDATE(),
				0
			)

		COMMIT TRANSACTION

		PRINT 'Inseriu na tabela de Fornecedor.';

		SELECT		TOP 1
					NomeFantasia,
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
		ORDER BY	F.FornecedorId DESC

	END
GO
