using System.Threading.Tasks;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface ILogsRepository
    {
        Task GravarLog(string mensagem, object entrada, object retorno, int id, bool email);

        Task GravarLog(string mensagem, object entrada, object retorno, bool email);

        Task GravarLog(string mensagem, object entrada, bool email);
    }
}
