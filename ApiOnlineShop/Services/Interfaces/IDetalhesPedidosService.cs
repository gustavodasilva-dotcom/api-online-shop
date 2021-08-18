using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IDetalhesPedidosService
    {
        Task<IEnumerable<DetalhesPedidoViewModel>> Obter(int id);
    }
}
