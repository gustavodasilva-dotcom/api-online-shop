using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IInformacoesContatosRepository
    {
        Task Deletar(int informacoesContatoId);
    }
}
