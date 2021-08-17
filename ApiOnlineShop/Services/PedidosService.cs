using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly IPedidosRepository _pedidosRepository;

        public PedidosService(IPedidosRepository pedidosRepository)
        {
            _pedidosRepository = pedidosRepository;
        }

        public async Task<PedidoViewModel> Obter(int id)
        {
            var query = $"[dbo].[uspObterPedido] {id}";

            var pedido = await _pedidosRepository.Obter(query);

            return pedido;
        }
    }
}
