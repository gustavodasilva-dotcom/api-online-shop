using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<FornecedorViewModel> Obter(string query);
        Task<FornecedorViewModel> ExecutarComando(string query);
        Task ExecutarComandoSemRetorno(string query);
    }
}
