using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Entities.Entities;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Services.Interfaces;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Services
{
    public class FornecedoresService : IFornecedoresService
    {
        private readonly IValidacoesService _validacoesService;

        private readonly IFornecedoresRepository _fornecedoresRepository;

        public FornecedoresService(IValidacoesService validacoesService, IFornecedoresRepository fornecedoresRepository)
        {
            _validacoesService = validacoesService;

            _fornecedoresRepository = fornecedoresRepository;
        }

        public async Task<FornecedorViewModel> Obter(string cnpj)
        {
            try
            {
                var fornecedor = await _fornecedoresRepository.Obter(cnpj);

                if (fornecedor == null)
                    throw new NotFoundException($"O CNPJ {cnpj} não corresponde a nenhum fornecedor.");

                return new FornecedorViewModel
                {
                    NomeFantasia = fornecedor.NomeFantasia,
                    RazaoSocial = fornecedor.RazaoSocial,
                    Cnpj = fornecedor.Cnpj,
                    Contato = fornecedor.Contato,
                    Cep = fornecedor.Cep,
                    Logradouro = fornecedor.Logradouro,
                    Numero = fornecedor.Numero,
                    Complemento = fornecedor.Complemento,
                    Bairro = fornecedor.Bairro,
                    Localidade = fornecedor.Localidade,
                    Uf = fornecedor.Uf,
                    Pais = fornecedor.Pais
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FornecedorViewModel> Inserir(FornecedorInputModel fornecedor)
        {
            try
            {
                var fornecedorInsert = new Fornecedor
                {
                    NomeFantasia = fornecedor.NomeFantasia,
                    RazaoSocial = fornecedor.RazaoSocial,
                    Cnpj = fornecedor.Cnpj,
                    Contato = fornecedor.Contato,
                    Endereco = new Endereco
                    {
                        Cep = fornecedor.Cep,
                        Logradouro = fornecedor.Logradouro,
                        Numero = fornecedor.Numero,
                        Complemento = fornecedor.Complemento,
                        Bairro = fornecedor.Bairro,
                        Localidade = fornecedor.Localidade,
                        Uf = fornecedor.Uf,
                        Pais = fornecedor.Pais
                    }
                };

                var fornecedorCadastrado = await _fornecedoresRepository.Obter(fornecedorInsert.Cnpj);

                if (fornecedorCadastrado != null)
                    throw new NotFoundException("Ocorreu um erro ao inserir o fornecedor, pois não foi possível encontrá-lo.");

                var enderecoID = await _fornecedoresRepository.Inserir(fornecedorInsert.Endereco);

                if (enderecoID == 0)
                    throw new Exception("Ocorreu um erro ao inserir o endereço.");

                await _fornecedoresRepository.Inserir(fornecedorInsert, enderecoID);

                fornecedorCadastrado = await _fornecedoresRepository.Obter(fornecedorInsert.Cnpj);

                if (fornecedorCadastrado == null)
                    throw new NotFoundException("Ocorreu um erro ao inserir o fornecedor, pois não foi possível encontrá-lo.");

                return new FornecedorViewModel
                {
                    NomeFantasia = fornecedorCadastrado.NomeFantasia,
                    RazaoSocial = fornecedorCadastrado.RazaoSocial,
                    Cnpj = fornecedorCadastrado.Cnpj,
                    Contato = fornecedorCadastrado.Contato,
                    Cep = fornecedorCadastrado.Cep,
                    Logradouro = fornecedorCadastrado.Logradouro,
                    Numero = fornecedorCadastrado.Numero,
                    Complemento = fornecedorCadastrado.Complemento,
                    Bairro = fornecedorCadastrado.Bairro,
                    Localidade = fornecedorCadastrado.Localidade,
                    Uf = fornecedorCadastrado.Uf,
                    Pais = fornecedorCadastrado.Pais
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        public async Task<FornecedorViewModel> Atualizar(string cnpj, FornecedorInputModel fornecedorUpdate)
        {
            var query = $"[dbo].[sp_AtualizarFornecedor] '{cnpj}', '{fornecedorUpdate.NomeFantasia}', '{fornecedorUpdate.RazaoSocial}', '{fornecedorUpdate.Cnpj}', '{fornecedorUpdate.Contato}', '{fornecedorUpdate.Cep}', '{fornecedorUpdate.Logradouro}', '{fornecedorUpdate.Numero}', '{fornecedorUpdate.Complemento}', '{fornecedorUpdate.Bairro}', '{fornecedorUpdate.Localidade}', '{fornecedorUpdate.Uf}', '{fornecedorUpdate.Pais}'";

            var fornecedor = await _fornecedoresRepository.ExecutarComando(query);

            return fornecedor;
        }
        */

        public async Task Deletar(string cnpj)
        {
            var query = $"[dbo].[sp_DeletarFornecedor] '{cnpj}'";

            await _fornecedoresRepository.ExecutarComandoSemRetorno(query);
        }

        public ErroViewModel ValidarDados(FornecedorInputModel model)
        {
            var mensagensDeErro = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(model.NomeFantasia)) mensagensDeErro.Add("O nome fantasia do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.RazaoSocial)) mensagensDeErro.Add("A razão social do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Cnpj)) mensagensDeErro.Add("O CNPJ do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Contato)) mensagensDeErro.Add("O contato do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Logradouro)) mensagensDeErro.Add("O logradouro do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Numero)) mensagensDeErro.Add("O número do endereço do fornecedor não pode estar vazio ou nulo.");

                if (_validacoesService.ENumerico(model.Numero)) mensagensDeErro.Add("O número residencial não é um tipo numérico.");

                if (string.IsNullOrEmpty(model.Complemento)) mensagensDeErro.Add("O complemento do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Bairro)) mensagensDeErro.Add("O bairro do endereço do fornecedor não pode estar vazio ou nulo.");
                if (string.IsNullOrEmpty(model.Localidade)) mensagensDeErro.Add("A localidade do endereço do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Uf)) mensagensDeErro.Add("A UF do endereço do fornecedor não pode estar vazia ou nula.");
                if (string.IsNullOrEmpty(model.Pais)) mensagensDeErro.Add("O país do endereço do fornecedor não pode estar vazio ou nulo.");

                if (!_validacoesService.ValidarCep(model.Cep)) mensagensDeErro.Add("O CEP informado não existe.");

                if (!_validacoesService.ValidarCnpj(model.Cnpj)) mensagensDeErro.Add("O CNPJ informado está inválido.");

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
