USE OnlineShop
GO

CREATE PROCEDURE [dbo].[sp_ObterCliente]
	@Cpf	VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para obter os dados de clientes.
Data de criação: 14-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT OFF;
		
		DECLARE	@Mensagem VARCHAR(255);


		IF (SELECT 1 FROM Cliente WHERE Cpf = @Cpf AND Excluido = 0) IS NULL
		BEGIN
			SET @Mensagem = 'Cliente não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Cliente WHERE Cpf = @Cpf AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'Cliente não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Cliente WHERE Cpf = @Cpf) IS NULL
		BEGIN
			SET @Mensagem = 'Cliente não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


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
		INNER JOIN	InformacoesContato	IC	ON C.InformacoesContatoId	= IC.InformacoesContatoId
		INNER JOIN	Endereco			E	ON C.EnderecoId				= E.EnderecoId
		WHERE		C.Cpf = @Cpf AND C.Excluido = 0 AND E.Excluido = 0 AND IC.Excluido = 0

	END
GO