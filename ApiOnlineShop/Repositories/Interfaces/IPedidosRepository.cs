using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IPedidosRepository
    {
        Task<PedidoTable> Obter(int id);

        Task<IEnumerable<ItemsTable>> ObterItems(int id);

        Task<int> InserirPedido(Pedido pedido);

        Task InserirDetalhesPedido(Pedido pedido, int pedidoID);

        Task<PedidoViewModel> ExecutarComando(string query);
    }
}
