using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface ILogsService
    {
        Task GravarLog(object jsonEntrada, object jsonRetorno, string mensagem, int id, bool email);

        Task GravarLog(object jsonEntrada, object jsonRetorno, string mensagem, bool email);

        Task GravarLog(object jsonEntrada, string mensagem, bool email);
    }
}
