USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_ObterFornecedor]
	@Cnpj	VARCHAR(255)
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para obter os dados de fornecedores.
Data de criação: 14-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;
		
		DECLARE	@Mensagem VARCHAR(255);


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj AND Excluido = 0) IS NULL
		BEGIN
			SET @Mensagem = 'Fornecedor não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj AND Excluido = 1) IS NOT NULL
		BEGIN
			SET @Mensagem = 'Fornecedor não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Fornecedor WHERE Cnpj = @Cnpj) IS NULL
		BEGIN
			SET @Mensagem = 'Fornecedor não consta em sistema.';
			PRINT @Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


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
		WHERE		Cnpj = @Cnpj AND F.Excluido = 0 AND E.Excluido = 0

	END
GO