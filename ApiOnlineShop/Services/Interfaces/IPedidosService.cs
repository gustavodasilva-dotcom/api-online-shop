using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IPedidosService
    {
        Task<PedidoViewModel> Obter(int id);

        Task<int> Inserir(PedidoInputModel pedido);
    }
}
