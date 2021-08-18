using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services
{
    public class DetalhesPedidosService : IDetalhesPedidosService
    {
        private readonly IDetalhesPedidosRepository _detalhesPedidosRepository;

        public DetalhesPedidosService(IDetalhesPedidosRepository detalhesPedidosRepository)
        {
            _detalhesPedidosRepository = detalhesPedidosRepository;
        }

        public async Task<IEnumerable<DetalhesPedidoViewModel>> Obter(int id)
        {
            var query = $"[dbo].[uspObterDetalhesPedido] {id}";

            var detalhesPedido = await _detalhesPedidosRepository.Obter(query);

            return detalhesPedido;
        }
    }
}
