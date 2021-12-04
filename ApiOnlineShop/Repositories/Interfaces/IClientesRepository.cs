using System.Threading.Tasks;
using ApiOnlineShop.Entities.Table;
using ApiOnlineShop.Entities.Entities;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IClientesRepository
    {
        Task<ClienteTable> Obter(string cpf);

        Task<int> Inserir(InformacoesContato informacoesContato);

        Task Inserir(Cliente cliente);

        Task Atualizar(InformacoesContato informacoesContato);

        Task Atualizar(Cliente cliente);

        Task Deletar(ClienteTable cliente);
    }
}
