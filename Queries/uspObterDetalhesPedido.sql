USE OnlineShop
GO

ALTER PROCEDURE [dbo].[uspObterDetalhesPedido]
	@PedidoId INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para obter os detalhes de um pedido.
Data de criação: 16-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem VARCHAR(255);


		IF (SELECT 1 FROM Pedido WHERE PedidoId = @PedidoId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'Esse pedido não existe no sistema.';
			PRINT	@Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		SET		@Mensagem = 'Pedido ' + CAST(@PedidoId AS VARCHAR(8)) +  ' encontrado na base de dados.';
		PRINT	@Mensagem;
		PRINT	'';
		SET		@Mensagem = NULL;


		IF (SELECT TOP 1 1 FROM DetalhesPedido WHERE PedidoId = @PedidoId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'Esse pedido não possui itens.';
			PRINT	@Mensagem;

			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		SET		@Mensagem = 'Pedido ' + CAST(@PedidoId AS VARCHAR(8)) +  ' possui itens.';
		PRINT	@Mensagem;
		PRINT	'';
		SET		@Mensagem = NULL;


		SELECT		P.PedidoId,
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
	END
GO