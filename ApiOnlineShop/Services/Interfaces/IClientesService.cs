using System.Threading.Tasks;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Models.InputModels;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IClientesService
    {
        Task<ClienteViewModel> Obter(string cpf);
        
        Task<ClienteViewModel> Inserir(ClienteInputModel cliente);
        
        Task<ClienteViewModel> Atualizar(string cpf, ClienteInputModel cliente);
        
        Task Deletar(string cpf);
    }
}
