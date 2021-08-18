using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IDetalhesPedidosRepository
    {
        Task<IEnumerable<DetalhesPedidoViewModel>> Obter(string query);
    }
}
