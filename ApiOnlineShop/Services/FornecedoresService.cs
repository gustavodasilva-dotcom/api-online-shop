using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services.Interfaces;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services
{
    public class FornecedoresService : IFornecedoresService
    {
        private readonly IFornecedoresRepository _fornecedoresRepository;

        public FornecedoresService(IFornecedoresRepository fornecedoresRepository)
        {
            _fornecedoresRepository = fornecedoresRepository;
        }

        public async Task<FornecedorViewModel> Obter(string cnpj)
        {
            var query = $"SELECT NomeFantasia, RazaoSocial, Cnpj, Contato, Cep, Logradouro, Numero, Complemento, Bairro, Localidade, Uf, Pais FROM Fornecedor F INNER JOIN Endereco E ON F.EnderecoId = E.EnderecoId WHERE Cnpj = '{cnpj}' AND F.Excluido = 0 AND E.Excluido = 0";

            var fornecedor = await _fornecedoresRepository.Obter(query);

            return fornecedor;
        }

        public async Task<FornecedorViewModel> Inserir(FornecedorInputModel fornecedorInsert)
        {
            var query = $"[dbo].[sp_CadastrarFornecedor] '{fornecedorInsert.NomeFantasia}', '{fornecedorInsert.RazaoSocial}', '{fornecedorInsert.Cnpj}', '{fornecedorInsert.Contato}', '{fornecedorInsert.Cep}', '{fornecedorInsert.Logradouro}', '{fornecedorInsert.Numero}', '{fornecedorInsert.Complemento}', '{fornecedorInsert.Bairro}', '{fornecedorInsert.Localidade}', '{fornecedorInsert.Uf}', '{fornecedorInsert.Pais}'";

            var fornecedor = await _fornecedoresRepository.ExecutarComando(query);

            return fornecedor;
        }

        public async Task<FornecedorViewModel> Atualizar(string cnpj, FornecedorInputModel fornecedorUpdate)
        {
            var query = $"[dbo].[sp_AtualizarFornecedor] '{cnpj}', '{fornecedorUpdate.NomeFantasia}', '{fornecedorUpdate.RazaoSocial}', '{fornecedorUpdate.Cnpj}', '{fornecedorUpdate.Contato}', '{fornecedorUpdate.Cep}', '{fornecedorUpdate.Logradouro}', '{fornecedorUpdate.Numero}', '{fornecedorUpdate.Complemento}', '{fornecedorUpdate.Bairro}', '{fornecedorUpdate.Localidade}', '{fornecedorUpdate.Uf}', '{fornecedorUpdate.Pais}'";

            var fornecedor = await _fornecedoresRepository.ExecutarComando(query);

            return fornecedor;
        }

        public async Task Deletar(string cnpj)
        {
            var query = $"[dbo].[sp_DeletarFornecedor] '{cnpj}'";

            await _fornecedoresRepository.ExecutarComandoSemRetorno(query);
        }
    }
}
