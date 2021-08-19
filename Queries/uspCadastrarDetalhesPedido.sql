USE OnlineShop
GO

ALTER PROCEDURE [dbo].[uspCadastrarDetalhesPedido]
	@PedidoId	INT,
	@ProdutoId	INT,
	@Quantidade INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para criar os detalhes (itens) de um pedido.
Data de criação: 18-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem VARCHAR(255);
		
		
		-- Validação do pedido.
		IF (SELECT 1 FROM Pedido WHERE PedidoId = @PedidoId AND Excluido = 0) IS NULL 
		BEGIN
			SET		@Mensagem = 'Pedido informado: ' + CAST(@PedidoId AS VARCHAR(8)) + '. Pedido não foi encontrado em sistema.';
			PRINT	@Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		SET		@Mensagem = 'Pedido ' + CAST(@PedidoId AS VARCHAR(8)) + ' foi encontrado em sistema.';
		PRINT	@Mensagem;


		-- Validação do produto.
		IF (SELECT 1 FROM Produto WHERE ProdutoId = @ProdutoId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'Produto informado: ' + CAST(@ProdutoId AS VARCHAR(8)) + '. Produto não foi encontrado em sistema.';
			PRINT	@Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END

		SET		@Mensagem = 'Produto ' + CAST(@ProdutoId AS VARCHAR(8)) + ' foi encontrado em sistema.';
		PRINT	@Mensagem;


		-- Validação da quantidade.
		IF @Quantidade <= 0
		BEGIN
			SET		@Mensagem = 'A quantidade não pode ser igual ou menor a 0.'
			PRINT	@Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		PRINT 'Inserindo item na tabela de DetalhesPedido: ';

		BEGIN TRANSACTION

			INSERT INTO DetalhesPedido
			VALUES
			(
				@Quantidade,
				@ProdutoId,
				@PedidoId,
				GETDATE(),
				0
			);

		COMMIT TRANSACTION


		PRINT 'Item inserido com sucesso na tabela DetalhesPedido.';

		SELECT		TOP 1
					P.PedidoId,
					DP.ProdutoId,
					Quantidade,
					PR.Nome,
					PR.Medida,
					CAST(PR.Preco AS VARCHAR(20)) AS Preco,
					CASE
						WHEN (F.NomeFantasia IS	NULL) OR (F.NomeFantasia = '') THEN F.RazaoSocial
						ELSE F.NomeFantasia
					END AS Fornecedor
		FROM		Pedido			P	(NOLOCK)
		INNER JOIN	DetalhesPedido	DP	(NOLOCK) on P.PedidoId = DP.PedidoId
		INNER JOIN	Produto			PR	(NOLOCK) on DP.ProdutoId = PR.ProdutoId
		INNER JOIN	Fornecedor		F	(NOLOCK) on PR.FornecedorId = F.FornecedorId
		WHERE		P.PedidoId = @PedidoId
		AND			P.Excluido = 0
		AND			DP.Excluido = 0
		ORDER BY	DP.PedidoId DESC;

	END
GO