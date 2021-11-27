using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IProdutosService
    {
        Task<IEnumerable<ProdutoViewModel>> Obter(int id, int tipo);

        Task<int> Inserir(ProdutoInputModel produto);
    }
}
