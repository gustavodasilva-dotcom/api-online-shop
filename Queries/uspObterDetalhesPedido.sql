USE OnlineShop

CREATE PROCEDURE [dbo].[uspObterDetalhesPedido]
	@PedidoId INT
AS
	BEGIN

/****************************************************************************************************************************
-- Basicamente, esta procedure terá como core o SELECT abaixo. Deve-se implementar validações antes de criá-la.
****************************************************************************************************************************/

		select		dp.ProdutoId,
					quantidade,
					pr.nome,
					pr.medida,
					CAST(pr.Preco AS VARCHAR(20)),
					case
						when (f.NomeFantasia IS		NULL) OR (f.NomeFantasia = '') then f.RazaoSocial
						else f.NomeFantasia
					end
		from		pedido			p	(nolock)
		inner join	detalhespedido	dp	(nolock) on p.PedidoId = dp.PedidoId
		inner join	Produto			pr	(nolock) on dp.ProdutoId = pr.ProdutoId
		inner join	Fornecedor		f	(nolock) on pr.FornecedorId = f.FornecedorId
		where		p.pedidoid = @PedidoId
	
	END
GO