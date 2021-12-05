using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Services.Interfaces;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Services
{
    public class ClientesService : IClientesService
    {
        private readonly IValidacoesService _validacoesService;

        private readonly IClientesRepository _clientesRepository;

        private readonly IEnderecosRepository _enderecosRepository;

        private readonly IInformacoesContatosRepository _informacoesContatosRepository;

        public ClientesService(IValidacoesService validacoesService, IClientesRepository clientesRepository, IEnderecosRepository enderecosRepository, IInformacoesContatosRepository informacoesContatosRepository)
        {
            _validacoesService = validacoesService;

            _clientesRepository = clientesRepository;

            _enderecosRepository = enderecosRepository;

            _informacoesContatosRepository = informacoesContatosRepository;
        }

        public async Task<ClienteViewModel> Obter(string cpf)
        {
            try
            {
                var cliente = await _clientesRepository.Obter(cpf);

                if (cliente == null)
                    throw new NotFoundException($"O CPF {cpf} não corresponde a um cliente.");

                return new ClienteViewModel
                {
                    ClienteId = cliente.ClienteId,
                    PrimeiroNome = cliente.PrimeiroNome,
                    NomeDoMeio = cliente.NomeDoMeio,
                    Sobrenome = cliente.Sobrenome,
                    Cpf = cliente.Cpf,
                    Telefone = cliente.Telefone,
                    Celular = cliente.Celular,
                    Email = cliente.Email,
                    Cep = cliente.Cep,
                    Logradouro = cliente.Logradouro,
                    Numero = cliente.Numero,
                    Complemento = cliente.Complemento,
                    Bairro = cliente.Bairro,
                    Localidade = cliente.Localidade,
                    Uf = cliente.Uf,
                    Pais = cliente.Pais
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteViewModel> Inserir(ClienteInputModel cliente)
        {
            try
            {
                if (await _clientesRepository.Obter(cliente.Cpf) != null)
                    throw new ConflictException($"O CPF {cliente.Cpf} já está cadastrado em sistema.");

                var clienteInsert = new Cliente
                {
                    PrimeiroNome = cliente.PrimeiroNome,
                    NomeDoMeio = cliente.NomeDoMeio,
                    Sobrenome = cliente.Sobrenome,
                    Cpf = cliente.Cpf,
                    Endereco = new Endereco
                    {
                        Cep = cliente.Cep,
                        Logradouro = cliente.Logradouro,
                        Numero = cliente.Numero,
                        Complemento = cliente.Complemento,
                        Bairro = cliente.Bairro,
                        Localidade = cliente.Localidade,
                        Uf = cliente.Uf,
                        Pais = cliente.Pais
                    },
                    InformacoesContato = new InformacoesContato
                    {
                        Telefone = cliente.Telefone,
                        Celular = cliente.Celular,
                        Email = cliente.Email
                    }
                };

                var enderecoID = await _enderecosRepository.Inserir(clienteInsert.Endereco);

                if (enderecoID == 0)
                    throw new Exception("Ocorreu um erro ao inserir o endereço do cliente.");

                var informacoesContatoID = await _clientesRepository.Inserir(clienteInsert.InformacoesContato);

                if (informacoesContatoID == 0)
                    throw new Exception("Ocorreu um erro ao inserir as informações de contato do cliente.");

                clienteInsert.Endereco.Codigo = enderecoID;
                clienteInsert.InformacoesContato.Codigo = informacoesContatoID;

                await _clientesRepository.Inserir(clienteInsert);

                return await Obter(clienteInsert.Cpf);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteViewModel> Atualizar(string cpf, ClienteInputModel cliente)
        {
            try
            {
                var dadosCliente = await _clientesRepository.Obter(cpf);

                if (dadosCliente == null)
                    throw new NotFoundException($"O CPF {cpf} não corresponde a um cliente.");

                var clienteUpdate = new Cliente
                {
                    Codigo = dadosCliente.ClienteId,
                    PrimeiroNome = cliente.PrimeiroNome,
                    NomeDoMeio = cliente.NomeDoMeio,
                    Sobrenome = cliente.Sobrenome,
                    Cpf = cliente.Cpf,
                    Endereco = new Endereco
                    {
                        Codigo = dadosCliente.EnderecoId,
                        Cep = cliente.Cep,
                        Logradouro = cliente.Logradouro,
                        Numero = cliente.Numero,
                        Complemento = cliente.Complemento,
                        Bairro = cliente.Bairro,
                        Localidade = cliente.Localidade,
                        Uf = cliente.Uf,
                        Pais = cliente.Pais
                    },
                    InformacoesContato = new InformacoesContato
                    {
                        Codigo = dadosCliente.EnderecoId,
                        Telefone = cliente.Telefone,
                        Celular = cliente.Celular,
                        Email = cliente.Email
                    }
                };

                await _enderecosRepository.Atualizar(clienteUpdate.Endereco);

                await _clientesRepository.Atualizar(clienteUpdate.InformacoesContato);

                await _clientesRepository.Atualizar(clienteUpdate);

                return await Obter(clienteUpdate.Cpf);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Deletar(string cpf)
        {
            try
            {
                var dadosCliente = await _clientesRepository.Obter(cpf);

                if (dadosCliente == null)
                    throw new NotFoundException($"O CPF {cpf} não corresponde a um cliente.");

                await _informacoesContatosRepository.Deletar(dadosCliente.InformacoesContatoId);

                await _enderecosRepository.Deletar(dadosCliente.EnderecoId);

                await _clientesRepository.Deletar(dadosCliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ErroViewModel ValidarDados(ClienteInputModel model)
        {
            var mensagensDeErro = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(model.PrimeiroNome)) mensagensDeErro.Add("O nome fantasia do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.NomeDoMeio)) mensagensDeErro.Add("A razão social do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Sobrenome)) mensagensDeErro.Add("O CNPJ do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Cpf)) mensagensDeErro.Add("O contato do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Celular)) mensagensDeErro.Add("O número do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Email)) mensagensDeErro.Add("O número do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Logradouro)) mensagensDeErro.Add("O logradouro do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Numero)) mensagensDeErro.Add("O número do endereço do fornecedor não pode estar vazio ou nulo.");

                if (_validacoesService.ENumerico(model.Numero)) mensagensDeErro.Add("O número residencial não é um tipo numérico.");
                
                if (!string.IsNullOrEmpty(model.Telefone))
                    if (_validacoesService.ENumerico(model.Numero)) mensagensDeErro.Add("O número de telefone não é um tipo numérico.");

                if (_validacoesService.ENumerico(model.Celular)) mensagensDeErro.Add("O número de celular não é um tipo numérico.");

                if (string.IsNullOrEmpty(model.Bairro)) mensagensDeErro.Add("O bairro do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Localidade)) mensagensDeErro.Add("A localidade do endereço do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Uf)) mensagensDeErro.Add("A UF do endereço do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Pais)) mensagensDeErro.Add("O país do endereço do fornecedor não pode estar vazio ou nulo.");

                if (!_validacoesService.ValidarUf(model.Uf)) mensagensDeErro.Add("O UF informada está inválida.");

                if (!_validacoesService.ValidarCep(model.Cep)) mensagensDeErro.Add("O CEP informado não existe.");

                if (!_validacoesService.ValidarCpf(model.Cpf)) mensagensDeErro.Add("O CNPJ informado está inválido.");

                if (_validacoesService.ValidarEmail(model.Email)) mensagensDeErro.Add("O e-mail informado está inválido.");

                return new ErroViewModel
                {
                    StatusCode = 400,
                    MensagensDeErro = mensagensDeErro
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ErroViewModel ValidarDados(string cpf, ClienteInputModel model)
        {
            var mensagensDeErro = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(model.PrimeiroNome)) mensagensDeErro.Add("O primeiro nome do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.NomeDoMeio)) mensagensDeErro.Add("O nome do meio do cliente não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Sobrenome)) mensagensDeErro.Add("O sobrenome do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Cpf)) mensagensDeErro.Add("O CPF do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Celular)) mensagensDeErro.Add("O celular do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Email)) mensagensDeErro.Add("O e-mail do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Cep)) mensagensDeErro.Add("O CEP do endereço do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Logradouro)) mensagensDeErro.Add("O logradouro do endereço do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Numero)) mensagensDeErro.Add("O número do endereço do cliente não pode estar vazio ou nulo.");

                if (_validacoesService.ENumerico(model.Numero)) mensagensDeErro.Add("O número residencial não é um tipo numérico.");

                if (!string.IsNullOrEmpty(model.Telefone))
                    if (_validacoesService.ENumerico(model.Numero)) mensagensDeErro.Add("O número de telefone não é um tipo numérico.");

                if (_validacoesService.ENumerico(model.Celular)) mensagensDeErro.Add("O número de celular não é um tipo numérico.");

                if (string.IsNullOrEmpty(model.Bairro)) mensagensDeErro.Add("O bairro do endereço do cliente não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Localidade)) mensagensDeErro.Add("A localidade do endereço do cliente não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Uf)) mensagensDeErro.Add("A UF do endereço do cliente não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Pais)) mensagensDeErro.Add("O país do endereço do cliente não pode estar vazio ou nulo.");

                if (!_validacoesService.ValidarUf(model.Uf)) mensagensDeErro.Add("O UF informada está inválida.");

                if (!_validacoesService.ValidarCep(model.Cep)) mensagensDeErro.Add("O CEP informado não existe.");

                if (!_validacoesService.ValidarCpf(model.Cpf)) mensagensDeErro.Add("O CNPJ informado está inválido.");

                if (!_validacoesService.ValidarCpf(cpf)) mensagensDeErro.Add("O CNPJ informado na rota está inválido.");

                if (_validacoesService.ValidarEmail(model.Email)) mensagensDeErro.Add("O e-mail informado está inválido.");

                return new ErroViewModel
                {
                    StatusCode = 400,
                    MensagensDeErro = mensagensDeErro
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
