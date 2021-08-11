USE OnlineShop
GO

/*************************************************************************************************************************************
********************* CORRIGIR PROC. ESTÁ INSERINDO A CHAVE ESTRANGEIRA ERRADA. POSSÍVELMENTE, ALTERAR TABELA. ***********************
**************************************************************************************************************************************/

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
		DECLARE @IdEndereco		UNIQUEIDENTIFIER;
		DECLARE @IdInfoContato	UNIQUEIDENTIFIER;
		

		PRINT 'Inserindo na tabela Endereco: ';

		INSERT INTO Endereco
		VALUES
		(
			NEWID(),
			@Cep,
			@Logradouro,
			@Numero,
			@Complemento,
			@Bairro,
			@Localidade,
			@Uf,
			@Pais,
			GETDATE()
		);

		PRINT 'Inseriu na tabela de Endereco.';


		SELECT @IdEndereco = EnderecoId FROM Endereco ORDER BY DataInsercao DESC;

		PRINT 'Id de Endereco cadastrado: ' + CAST(@IdEndereco AS VARCHAR(255))


		PRINT 'Inserindo na tabela de InformacoesContato: ';

		INSERT INTO InformacoesContato
		VALUES
		(
			NEWID(),
			@Telefone,
			@Celular,
			@Email,
			GETDATE()
		);

		PRINT 'Inseriu na tabela de InformacoesContato.';


		PRINT 'Inserindo na tabela de Cliente: ';

		SELECT @IdInfoContato = InformacoesContatoId FROM InformacoesContato ORDER BY DataInsercao DESC;

		PRINT 'Id de InformacoesContato cadastrado: ' + CAST(@IdInfoContato AS VARCHAR(255))


		INSERT INTO Cliente
		VALUES
		(
			NEWID(),
			@PrimeiroNome,
			@NomeDoMeio,
			@Sobrenome,
			@Cpf,
			@IdEndereco,
			@IdInfoContato,
			GETDATE()
		);

		PRINT 'Inseriu na tabela de Cliente.';


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
		INNER JOIN	InformacoesContato  IC ON C.InformacoesContatoId = IC.InformacoesContatoId
		INNER JOIN	Endereco			E  ON C.EnderecoId			 = E.EnderecoId
		ORDER BY	C.Codigo DESC;

	END
GO