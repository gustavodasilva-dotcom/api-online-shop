USE OnlineShop
GO

ALTER PROCEDURE [dbo].[uspObterPedido]
	@PedidoId INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para obter um único pedido.
Data de criação: 16-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT OFF;

		DECLARE @Mensagem VARCHAR(255);
	
		IF (SELECT 1 FROM Pedido WHERE PedidoId = @PedidoId AND Excluido = 0) IS NOT NULL
			BEGIN

				SELECT		TOP 1
							P.PedidoId,
							C.ClienteId,
							CONCAT(C.PrimeiroNome, ' ', C.NomeDoMeio, ' ', C.Sobrenome) AS Cliente,
							C.CPF,
							CASE
								WHEN (e.Complemento = '') OR (e.Complemento IS NULL)
								THEN
									CONCAT(E.Logradouro,	', ',
											E.Numero,		' - ',
											E.Bairro,		', ',
											E.Localidade,	' - ',
											E.Uf,			', ',
											E.Pais,			' - ',
											E.Cep)
								ELSE
									CONCAT(E.Logradouro,	', ',
											E.Numero,		' - ',
											E.Complemento,	' - ',
											E.Bairro,		', ',
											E.Localidade,	' - ',
											E.Uf,			', ',
											E.Pais,			' - ',
											E.Cep)
								END
							AS Endereco,
							DataCompra
				FROM		Pedido		P (NOLOCK)
				INNER JOIN	Cliente		C (NOLOCK) ON P.ClienteId	= C.ClienteId
				INNER JOIN	Endereco	E (NOLOCK) ON C.EnderecoId	= E.EnderecoId
				WHERE		PedidoId = 10000001;

				SET		@Mensagem = 'Pedido encontrado na base de dados!';
				PRINT	@Mensagem;

			END
		ELSE
			BEGIN
				SET		@Mensagem = 'O pedido não foi encontrado na base de dados.';
				PRINT	@Mensagem;

				RAISERROR(@Mensagem, 20, -1) WITH LOG;
			END
	
	END
GO