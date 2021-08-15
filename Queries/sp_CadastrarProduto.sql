USE OnlineShop
GO

ALTER PROCEDURE [dbo].[sp_CadastrarProduto]
	@Nome			VARCHAR(255),
	@Medidas		VARCHAR(255),
	@Preco			FLOAT(2),
	@CategoriaId	INT,
	@FornecedorId	INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para cadastrar produtos.
Data de criação: 14-08-2021
**************************************************************************************************************************************/
		DECLARE @Mensagem VARCHAR(255);


		IF @Preco <= 0
		BEGIN
			SET		@Mensagem = 'O preço do produto não pode ser igual ou menor a 0.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF @Preco IS NULL
		BEGIN
			SET		@Mensagem = 'O preço do produto não pode ser nulo.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Categoria WHERE CategoriaId = @CategoriaId) IS NULL
		BEGIN
			SET		@Mensagem = 'A categoria informada não existe.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		IF (SELECT 1 FROM Categoria WHERE CategoriaId = @CategoriaId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'A categoria informada não existe.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		IF (SELECT 1 FROM Categoria WHERE CategoriaId = @CategoriaId AND Excluido = 1) IS NOT NULL
			BEGIN
				SET		@Mensagem = 'A categoria informada não existe.';
				PRINT	@Mensagem;
				PRINT	'';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END
		ELSE
			BEGIN
				SET		@Mensagem = 'Categoria encontrada na base de dados.';
				PRINT	@Mensagem;
				PRINT	'';
				
				SET		@Mensagem = NULL;
			END


		IF (SELECT 1 FROM Fornecedor WHERE FornecedorId = @FornecedorId) IS NULL
		BEGIN
			SET		@Mensagem = 'O fornecedor informado não existe.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		IF (SELECT 1 FROM Fornecedor WHERE FornecedorId = @FornecedorId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'O fornecedor informado não existe.';
			PRINT	@Mensagem;
			PRINT	'';

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		IF (SELECT 1 FROM Fornecedor WHERE FornecedorId = @FornecedorId AND Excluido = 1) IS NOT NULL
			BEGIN
				SET		@Mensagem = 'O fornecedor informado não existe.';
				PRINT	@Mensagem;
				PRINT	'';

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END
		ELSE
			BEGIN
				SET		@Mensagem = 'Fornecedor encontrado na base de dados.';
				PRINT	@Mensagem;
				PRINT	'';
				
				SET		@Mensagem = NULL;
			END


		PRINT 'Inserindo produto na tabela Produto:';
		PRINT '';

		BEGIN TRANSACTION

			INSERT INTO Produto
			VALUES
			(
				@Nome,
				@Medidas,
				@Preco,
				@CategoriaId,
				@FornecedorId,
				GETDATE(),
				0
			);

		COMMIT TRANSACTION


		PRINT 'Produto inserido com sucesso: '


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
		INNER JOIN	Fornecedor	F ON P.FornecedorId = F.FornecedorId
		INNER JOIN	Categoria	C ON P.CategoriaId	= C.CategoriaId
		ORDER BY	P.DataInsercao DESC;	

	END
GO