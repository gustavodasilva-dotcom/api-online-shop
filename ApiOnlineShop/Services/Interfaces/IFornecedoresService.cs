using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IFornecedoresService
    {
        Task<FornecedorViewModel> Obter(string cnpj);
        
        Task<FornecedorViewModel> Inserir(FornecedorInputModel fornecedor);
        
        //Task<FornecedorViewModel> Atualizar(string cnpj, FornecedorInputModel fornecedorUpdate);
        
        Task Deletar(string cnpj);

        ErroViewModel ValidarDados(FornecedorInputModel model);
    }
}
