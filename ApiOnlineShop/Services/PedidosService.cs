using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly IPedidosRepository _pedidosRepository;

        public PedidosService(IPedidosRepository pedidosRepository)
        {
            _pedidosRepository = pedidosRepository;
        }

        public async Task<PedidoViewModel> Obter(int id)
        {
            try
            {
                var detalhesPedido = new List<DetalhesPedidoViewModel>();

                var pedido = await _pedidosRepository.Obter(id);

                if (pedido == null)
                    throw new NotFoundException($"O pedido {id} não foi encontrado na base de dados.");

                var items = await _pedidosRepository.ObterItems(id);

                if (items == null)
                    throw new NotFoundException($"O pedido {id} não possui items.");

                foreach (var item in items)
                {
                    detalhesPedido.Add(new DetalhesPedidoViewModel
                    {
                        PedidoId = item.PedidoID,
                        ProdutoId = item.ProdutoID,
                        Quantidade = item.Quantidade,
                        Nome = item.Nome,
                        Medida = item.Medida,
                        Preco = item.Preco,
                        Fornecedor = item.Fornecedor
                    });
                }

                return new PedidoViewModel
                {
                    PedidoId = pedido.PedidoID,
                    ClienteId = pedido.ClienteID,
                    Cliente = pedido.Cliente,
                    Cpf = pedido.Cpf,
                    Endereco = new EnderecoViewModel
                    {
                        Cep = pedido.Cep,
                        Logradouro = pedido.Logradouro,
                        Complemento = pedido.Complemento,
                        Bairro = pedido.Bairro,
                        Localidade = pedido.Localidade,
                        Uf = pedido.Uf,
                        Pais = pedido.Pais
                    },
                    DetalhesPedido = detalhesPedido
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Inserir(PedidoInputModel pedido)
        {
            try
            {
                var detalhesInsert = new List<DetalhesPedido>();

                foreach (var detalhe in pedido.DetalhesPedido)
                {
                    detalhesInsert.Add(new DetalhesPedido
                    {
                        Produto = new Produto
                        {
                            Codigo = detalhe.ProdutoId,
                            Quantidade = detalhe.Quantidade
                        }
                    });
                };

                var pedidoInsert = new Pedido
                {
                    DataCompra = pedido.DataCompra,
                    Cliente = new Cliente
                    {
                        Codigo = pedido.ClienteId
                    },
                    DetalhesPedido = detalhesInsert
                };

                var pedidoID = await _pedidosRepository.InserirPedido(pedidoInsert);

                if (pedidoID != 0)
                {
                    await _pedidosRepository.InserirDetalhesPedido(pedidoInsert, pedidoID);

                    return pedidoID;
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao inserir o pedido.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
