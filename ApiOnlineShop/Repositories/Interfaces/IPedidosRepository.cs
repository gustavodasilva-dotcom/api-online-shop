using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IPedidosRepository
    {
        Task<PedidoViewModel> Obter(string query);
    }
}
