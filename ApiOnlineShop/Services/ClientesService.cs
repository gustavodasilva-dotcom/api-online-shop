using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services
{
    public class ClientesService : IClientesService
    {
        private readonly IClientesRepository _clientesRepository;

        public ClientesService(IClientesRepository clientesRepository)
        {
            _clientesRepository = clientesRepository;
        }

        public async Task<ClienteViewModel> Obter(string cpf)
        {
            var query = $"SELECT C.PrimeiroNome, C.NomeDoMeio, C.Sobrenome, C.Cpf, IC.Telefone, IC.Celular, IC.Email, E.Cep, E.Logradouro, E.Numero, E.Complemento, E.Bairro, E.Localidade, E.Uf, E.Pais FROM Cliente C INNER JOIN InformacoesContato IC  ON C.InformacoesContatoId = IC.InformacoesContatoId INNER JOIN Endereco E ON C.EnderecoId = E.EnderecoId WHERE C.Cpf = '{cpf}'";

            var cliente = await _clientesRepository.Obter(query);

            if (cliente == null)
                throw new Exception();

            return cliente;
        }

        public async Task<ClienteViewModel> Inserir(ClienteInputModel clienteInsert)
        {
            var procedure = $"[dbo].[sp_CadastrarCliente] '{clienteInsert.PrimeiroNome}', '{clienteInsert.NomeDoMeio}', '{clienteInsert.Sobrenome}', '{clienteInsert.Cpf}', '{clienteInsert.Telefone}', '{clienteInsert.Celular}', '{clienteInsert.Email}', '{clienteInsert.Cep}', '{clienteInsert.Logradouro}', '{clienteInsert.Numero}', '{clienteInsert.Complemento}', '{clienteInsert.Bairro}', '{clienteInsert.Localidade}', '{clienteInsert.Uf}', '{clienteInsert.Pais}'";

            var cliente = await _clientesRepository.ExecutarComando(procedure);

            return cliente;
        }
    }
}
