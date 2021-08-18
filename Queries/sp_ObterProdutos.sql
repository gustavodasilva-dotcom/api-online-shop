USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_ObterProdutos]
	@Id		INT,
	@Tipo	INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para obter um único produto ou uma lista de produtos.
Data de criação: 15-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem VARCHAR(255);


		IF (@Tipo > 2) OR (@Tipo <= 0)
		BEGIN
			SET @Mensagem = 'O tipo retorno informado não está especificado.';
			PRINT @Mensagem;
			PRINT '';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF LEN(@Id) > 1
		BEGIN
			IF (SELECT 1 FROM Produto WHERE ProdutoId = @Id AND Excluido = 0) IS NULL
				BEGIN
					SET @Mensagem = 'O id informado não corresponde a nenhum produto.';
					PRINT @Mensagem;
					PRINT '';

					RAISERROR(@Mensagem, 20, -1) WITH LOG;
				END
			ELSE
				BEGIN
					SET @Mensagem = 'Produto localizado com o id informado.';
					PRINT @Mensagem;
					PRINT '';

					SET @Mensagem = NULL;
				END
		END


		IF @Tipo = 1
		BEGIN
			
			SELECT		P.ProdutoId		AS 'ProdutoId',
						P.Nome			AS 'Nome',
						P.Medida		AS 'Medida',
						P.Preco			AS 'Preco',
						C.Nome			AS 'Categoria',
						C.Descricao		AS 'Descricao',
						F.RazaoSocial	AS 'Fornecedor',
						F.Cnpj			AS 'Cnpj'
			FROM		Produto		P
			INNER JOIN	Fornecedor	F	ON P.FornecedorId	= F.FornecedorId
			INNER JOIN	Categoria	C	ON P.CategoriaId	= C.CategoriaId
			WHERE		C.CategoriaId = @Id
			 AND		C.Excluido = 0
			 AND		P.Excluido = 0
			 AND		F.Excluido = 0;
		
		END


		IF @Tipo = 2
		BEGIN
			
			SELECT		TOP 1
						P.ProdutoId		AS 'ProdutoId',
						P.Nome			AS 'Nome',
						P.Medida		AS 'Medida',
						P.Preco			AS 'Preco',
						C.Nome			AS 'Categoria',
						C.Descricao		AS 'Descricao',
						F.RazaoSocial	AS 'Fornecedor',
						F.Cnpj			AS 'Cnpj'
			FROM		Produto		P
			INNER JOIN	Fornecedor	F	ON P.FornecedorId	= F.FornecedorId
			INNER JOIN	Categoria	C	ON P.CategoriaId	= C.CategoriaId
			WHERE		P.ProdutoId = @Id
			 AND		C.Excluido = 0
			 AND		P.Excluido = 0
			 AND		F.Excluido = 0;
		
		END

	END
GO