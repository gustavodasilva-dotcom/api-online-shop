using System.Threading.Tasks;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Entities.Entities;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IFornecedoresRepository
    {
        Task<FornecedorTable> Obter(string cnpj);

        Task Inserir(Fornecedor fornecedor, int enderecoID);

        Task Atualizar(Fornecedor fornecedor);

        Task Deletar(FornecedorTable fornecedor);
    }
}
