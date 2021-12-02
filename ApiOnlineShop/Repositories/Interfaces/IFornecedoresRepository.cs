using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Entities.Entities;
using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<FornecedorTable> Obter(string cnpj);

        Task<int> Inserir(Endereco endereco);

        Task Inserir(Fornecedor fornecedor, int enderecoID);

        Task ExecutarComandoSemRetorno(string query);
    }
}
