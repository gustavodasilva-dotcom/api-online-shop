using System.Threading.Tasks;
using ApiOnlineShop.Entities.Entities;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IEnderecosRepository
    {
        Task<int> Inserir(Endereco endereco);

        Task Atualizar(Endereco endereco);

        Task Deletar(int enderecoId);
    }
}
