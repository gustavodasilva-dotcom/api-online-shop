using System.Threading.Tasks;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Models.InputModels;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IFornecedoresService
    {
        Task<FornecedorViewModel> Obter(string cnpj);
        
        Task<FornecedorViewModel> Inserir(FornecedorInputModel fornecedor);
        
        Task<FornecedorViewModel> Atualizar(string cnpj, FornecedorInputModel fornecedor);
        
        Task Deletar(string cnpj);

        ErroViewModel ValidarDados(FornecedorInputModel model);

        ErroViewModel ValidarDados(string cnpj, FornecedorInputModel model);
    }
}
