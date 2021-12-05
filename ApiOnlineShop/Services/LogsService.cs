using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ApiOnlineShop.Services.Interfaces;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Services
{
    public class LogsService : ILogsService
    {
        private readonly ILogsRepository _logsRepository;

        public LogsService(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        public async Task GravarLog(object jsonEntrada, object jsonRetorno, string mensagem, int id, bool email)
        {
            try
            {
                var entrada = ConverterModelParaJson(jsonEntrada);

                var retorno = ConverterModelParaJson(jsonRetorno);

                var retornoEmail = email ? 1 : 0;

                await _logsRepository.GravarLog(mensagem, entrada, retorno, id, email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GravarLog(object jsonEntrada, string mensagem, bool email)
        {
            try
            {
                var entrada = ConverterModelParaJson(jsonEntrada);

                var retornoEmail = email ? 1 : 0;

                await _logsRepository.GravarLog(mensagem, entrada, email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ConverterModelParaJson(object model)
        {
            try
            {
                return JsonConvert.SerializeObject(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
