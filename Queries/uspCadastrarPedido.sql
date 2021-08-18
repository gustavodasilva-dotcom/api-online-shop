USE OnlineShop
GO

ALTER PROCEDURE [dbo].[uspCadastrarPedido]
	@DataCompra	DATETIME,
	@ClienteId	INT
AS
	BEGIN
/*************************************************************************************************************************************
Descrição......: Procedure utilizada para criar o cabeçalho de um pedido.
Data de criação: 17-08-2021
**************************************************************************************************************************************/
		SET NOCOUNT ON;

		DECLARE @Mensagem VARCHAR(255);


		IF (@ClienteId IS NULL) OR (@ClienteId = 0)
		BEGIN
			SET		@Mensagem = 'O id do cliente não pode ser nulo ou zero.';
			PRINT	@Mensagem;
			
			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		IF (SELECT 1 FROM Cliente WHERE ClienteId = @ClienteId AND Excluido = 0) IS NULL
		BEGIN
			SET		@Mensagem = 'Cliente não encontrado em sistema.';
			PRINT	@Mensagem;
			
			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		SET		@Mensagem = 'Cliente encontrado em sistema.';
		PRINT	@Mensagem;
		PRINT	'';
		SET		@Mensagem = NULL;


		IF @DataCompra > GETDATE()
		BEGIN
			SET		@Mensagem = 'A data da compra não pode ser maior que a data de hoje.';
			PRINT	@Mensagem;
			
			RAISERROR(@Mensagem, 20, -1) WITH LOG;
		END


		INSERT INTO Pedido
		VALUES
		(
			@DataCompra,
			@ClienteId,
			GETDATE(),
			0
		);
	
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
		ORDER BY	PedidoId DESC;

	END
GO