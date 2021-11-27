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
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;

        public ProdutosService(IProdutosRepository produtosRepository)
        {
            _produtosRepository = produtosRepository;
        }

        public async Task<IEnumerable<ProdutoViewModel>> Obter(int id, int tipo)
        {
            var query = $"[dbo].[sp_ObterProdutos] {id}, {tipo}";

            var produtos = await _produtosRepository.Obter(query);

            return produtos;
        }

        public async Task<int> Inserir(ProdutoInputModel produto)
        {
            try
            {
                var produtoInsert = new Produto
                {
                    Nome = produto.Nome,
                    Medida = produto.Medida,
                    Preco = produto.Preco,
                    Categoria = new Categoria
                    {
                        Codigo = produto.CategoriaId
                    },
                    Fornecedor = new Fornecedor
                    {
                        Codigo = produto.FornecedorId
                    },
                    Base64 = produto.Base64
                };

                if (!Base64Valido(produto.Base64))
                    throw new UnprocessableEntityException("O Base64 informado está inválido.");

                var produtoId = await _produtosRepository.Inserir(produtoInsert);

                if (produtoId != 0)
                {
                    await _produtosRepository.InserirBase64(produto.Base64, produtoId);

                    return produtoId;
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao inserir as informações do produto.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Base64Valido(string base64)
        {
            try
            {
                var buffer = new Span<byte>(new byte[base64.Length]);

                return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
