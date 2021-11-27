using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Entities.Enums;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ProdutoViewModel>> Obter(int categoriaId)
        {
            try
            {
                var categoriaValida = CategoriaValida(categoriaId);

                if (categoriaValida.Equals(Categorias.Categoria_invalida))
                    throw new NotFoundException("A categoria de produto informada não é válida.");

                var produtos = await _produtosRepository.Obter(categoriaId);

                if (!produtos.Any())
                    throw new NotFoundException("Não foram encontrados produtos para essa categoria.");

                return produtos.Select(p => new ProdutoViewModel
                {
                    ProdutoID = p.ProdutoID,
                    Nome = p.Nome,
                    Medida = p.Medida,
                    Preco = p.Preco,
                    Base64 = p.Base64 ?? "Não há imagem cadastrada.",
                    CategoriaID = p.CategoriaID,
                    Categoria = p.Categoria,
                    Descricao = p.Descricao,
                    Fornecedor = p.Fornecedor,
                    Cnpj = p.Cnpj
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoViewModel> Obter(int produtoId, int categoriaId)
        {
            try
            {
                var categoriaValida = CategoriaValida(categoriaId);

                if (categoriaValida.Equals(Categorias.Categoria_invalida))
                    throw new NotFoundException("A categoria de produto informada não é válida.");

                var produto = await _produtosRepository.Obter(produtoId, categoriaId);

                if (produto == null)
                    throw new NotFoundException("O produto informado não existe na base de dados.");

                return new ProdutoViewModel
                {
                    ProdutoID = produto.ProdutoID,
                    Nome = produto.Nome,
                    Medida = produto.Medida,
                    Preco = produto.Preco,
                    Base64 = produto.Base64 ?? "Não há imagem cadastrada.",
                    CategoriaID = produto.CategoriaID,
                    Categoria = produto.Categoria,
                    Descricao = produto.Descricao,
                    Fornecedor = produto.Fornecedor,
                    Cnpj = produto.Cnpj
                };
            }
            catch (Exception)
            {
                throw;
            }
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

                var categoriaValida = CategoriaValida(produtoInsert.Codigo);

                if (categoriaValida.Equals(Categorias.Categoria_invalida))
                    throw new NotFoundException("A categoria de produto informada não é válida.");

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

        private static bool Base64Valido(string base64)
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

        private static Categorias CategoriaValida(int categoriaId)
        {
            try
            {
                var categorias = new Categorias();

                categorias = Categorias.Categoria_invalida;

                switch (categoriaId)
                {
                    case 1:
                        categorias = Categorias.Moda_beleza_e_perfumaria;
                        break;
                    case 2:
                        categorias = Categorias.Informatica_e_tablets;
                        break;
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
