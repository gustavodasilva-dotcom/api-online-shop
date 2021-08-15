using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IProdutosRepository
    {
        Task<IEnumerable<ProdutoViewModel>> Obter(string query);
       
        Task<ProdutoViewModel> ExecutarComando(string query);
    }
}
