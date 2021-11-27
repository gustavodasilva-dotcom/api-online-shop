using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IProdutosRepository
    {
        Task<IEnumerable<ProdutoTable>> Obter(int categoriaId);

        Task<ProdutoTable> Obter(int produtoId, int categoriaId);

        Task<int> Inserir(Produto produto);

        Task InserirBase64(string base64, int produtoId);

        Task<ProdutoViewModel> ExecutarComando(string query);
    }
}
