using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IClientesRepository
    {
        Task<ClienteViewModel> Obter(string query);
        Task<ClienteViewModel> ExecutarComando(string query);
    }
}
