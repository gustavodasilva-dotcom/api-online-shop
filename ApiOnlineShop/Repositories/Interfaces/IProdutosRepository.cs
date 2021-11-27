using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IProdutosRepository
    {
        Task<IEnumerable<ProdutoViewModel>> Obter(string query);

        Task<int> Inserir(Produto produto);

        Task InserirBase64(string base64, int produtoId);

        Task<ProdutoViewModel> ExecutarComando(string query);
    }
}
