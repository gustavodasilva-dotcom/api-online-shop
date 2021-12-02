using System;
using RestSharp;
using Microsoft.Extensions.Configuration;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Repositories
{
    public class ValidacoesRepository : IValidacoesRepository
    {
        private readonly string _viaCepEndpoint;

        public ValidacoesRepository(IConfiguration configuration)
        {
            _viaCepEndpoint = configuration.GetConnectionString("ViaCep");
        }

        public bool ValidarCep(string cep)
        {
            try
            {
                var client = new RestClient(_viaCepEndpoint + $"{cep}/json/")
                {
                    Timeout = -1
                };
                
                var request = new RestRequest(Method.GET);
                
                request.AddParameter("application/json", ParameterType.RequestBody);
                
                IRestResponse response = client.Execute(request);

                if (response.Content.Contains("erro"))
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
