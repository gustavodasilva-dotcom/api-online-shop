using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IPedidosService
    {
        Task<PedidoViewModel> Obter(int id);
    }
}
