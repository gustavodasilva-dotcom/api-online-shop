using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
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
    }
}
