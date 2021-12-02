using System.Threading.Tasks;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Entities.Entities;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<FornecedorTable> Obter(string cnpj);

        Task<int> Inserir(Endereco endereco);

        Task Inserir(Fornecedor fornecedor, int enderecoID);

        Task Atualizar(Endereco endereco);

        Task Atualizar(Fornecedor fornecedor);

        Task Deletar(FornecedorTable fornecedor);
    }
}
