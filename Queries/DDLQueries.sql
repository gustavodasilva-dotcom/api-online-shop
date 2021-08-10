/*************************************************************************************************************************************
** SCRIPT DE CRIAÇÃO DO BANCO DE DADOS DA API ONLINESHOP *****************************************************************************
**************************************************************************************************************************************/
DROP DATABASE IF EXISTS OnlineShop;
CREATE DATABASE OnlineShop;

USE OnlineShop;


DROP TABLE IF EXISTS Endereco;
CREATE TABLE Endereco
(
	EnderecoId		UNIQUEIDENTIFIER	NOT NULL,
	Codigo			INT					NOT NULL	IDENTITY(1000001, 1),
	Cep				VARCHAR(255)		NOT NULL,
	Logradouro		VARCHAR(255)		NOT NULL,
	Numero			VARCHAR(255)		NOT NULL,
	Complemento		VARCHAR(255)		NOT NULL,
	Bairro			VARCHAR(255)		NOT NULL,
	Localidade		VARCHAR(255)		NOT NULL,
	Uf				CHAR(2)				NOT NULL,
	Pais			VARCHAR(255)		NOT NULL

	CONSTRAINT PK_EnderecoId PRIMARY KEY(EnderecoId),
);

DROP TABLE IF EXISTS InformacoesContato
CREATE TABLE InformacoesContato
(
	InformacoesContatoId	UNIQUEIDENTIFIER	NOT NULL,
	Codigo					INT					NOT NULL	IDENTITY(1000001, 1),
	Telefone				VARCHAR(15),
	Celular					VARCHAR(15)			NOT NULL,
	Email					VARCHAR(255)		NOT NULL

	CONSTRAINT PK_InformacoesContatoId PRIMARY KEY(InformacoesContatoId)
)

DROP TABLE IF EXISTS Cliente;
CREATE TABLE Cliente
(
	ClienteId				UNIQUEIDENTIFIER	NOT NULL,
	Codigo					INT					NOT NULL	IDENTITY(1000001, 1),
	PrimeiroNome			VARCHAR(255)		NOT NULL,
	NomeDoMeio				VARCHAR(255)		NOT NULL,
	Sobrenome				VARCHAR(255)		NOT NULL,
	Cpf						VARCHAR(255)		NOT NULL,
	EnderecoId				UNIQUEIDENTIFIER	NOT NULL,
	InformacoesContatoId	UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT PK_ClienteId PRIMARY KEY(ClienteId),

	CONSTRAINT FK_Cliente_EnderecoId FOREIGN KEY(EnderecoId)
	REFERENCES Endereco(EnderecoId),

	CONSTRAINT FK_Cliente_InformacoesContatoId FOREIGN KEY(InformacoesContatoId)
	REFERENCES InformacoesContato(InformacoesContatoId)
);

DROP TABLE IF EXISTS Fornecedor;
CREATE TABLE Fornecedor
(
	FornecedorId		UNIQUEIDENTIFIER	NOT NULL,
	Codigo				INT					NOT NULL	IDENTITY(1000001, 1),
	NomeFantasia		VARCHAR(255)		NOT NULL,
	RazaoSocial			VARCHAR(255)		NOT NULL,
	Cnpj				CHAR(14)			NOT NULL,
	Contato				VARCHAR(255)		NOT NULL,
	EnderecoId			UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT PK_FornecedorId PRIMARY KEY(FornecedorId),

	CONSTRAINT FK_Fornecedor_EnderecoId FOREIGN KEY(EnderecoId)
	REFERENCES Endereco(EnderecoId)
);

DROP TABLE IF EXISTS Categoria
CREATE TABLE Categoria
(
	CategoriaId		UNIQUEIDENTIFIER	NOT NULL,
	Codigo			INT					NOT NULL	IDENTITY(1000001, 1),
	Nome			VARCHAR(255)		NOT NULL,
	Descricao		VARCHAR(255)		NOT NULL

	CONSTRAINT PK_CategoriaId PRIMARY KEY(CategoriaId)
);

DROP TABLE IF EXISTS Produto;
CREATE TABLE Produto
(
	ProdutoId		UNIQUEIDENTIFIER	NOT NULL,
	Codigo			INT					NOT NULL	IDENTITY(1000001, 1),
	Nome			VARCHAR(255)		NOT NULL,
	Medida			VARCHAR(255)		NOT NULL,
	Preco			FLOAT(2)			NOT NULL,
	CategoriaId		UNIQUEIDENTIFIER	NOT NULL,
	FornecedorId	UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT PK_ProdutoId PRIMARY KEY(ProdutoId),

	CONSTRAINT FK_Produto_CategoriaId FOREIGN KEY(CategoriaId)
	REFERENCES Categoria(CategoriaId),

	CONSTRAINT FK_Produto_FornecedorId FOREIGN KEY(FornecedorId)
	REFERENCES Fornecedor(FornecedorId)
);

DROP TABLE IF EXISTS Pedido;
CREATE TABLE Pedido
(
	PedidoId		UNIQUEIDENTIFIER	NOT NULL,
	Codigo			INT					NOT NULL	IDENTITY(1000001, 1),
	DataCompra		DATETIME			NOT NULL,
	ClienteId		UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT PK_PedidoId PRIMARY KEY(PedidoId),

	CONSTRAINT FK_Pedido_ClienteId FOREIGN KEY(ClienteId)
	REFERENCES Cliente(ClienteId)
);

DROP TABLE IF EXISTS DetalhesPedido;
CREATE TABLE DetalhesPedido
(
	DetalhesPedidoId	UNIQUEIDENTIFIER	NOT NULL,
	Codigo				INT					NOT NULL	IDENTITY(1000001, 1),
	Quantidade			INT					NOT NULL,
	ProdutoId			UNIQUEIDENTIFIER	NOT NULL,
	PedidoId			UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT PK_DetalhesPedidoId PRIMARY KEY(DetalhesPedidoId),

	CONSTRAINT FK_DetalhesPedido_ProdutoId FOREIGN KEY(ProdutoId)
	REFERENCES Produto(ProdutoId),

	CONSTRAINT FK_DetalhesPedido_PedidoId FOREIGN KEY(PedidoId)
	REFERENCES Pedido(PedidoId)
);