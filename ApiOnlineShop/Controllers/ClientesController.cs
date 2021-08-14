using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;

        public ClientesController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        [HttpGet("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Obter([FromRoute] string cpf)
        {
            try
            {
                var cliente = await _clientesService.Obter(cpf);

                return StatusCode(200, cliente);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("Cliente não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Cliente não consta em sistema.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteViewModel>> Inserir([FromBody] ClienteInputModel clienteInsert)
        {
            try
            {
                var cliente = await _clientesService.Inserir(clienteInsert);

                return StatusCode(201, cliente);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("já consta em sistema."))
                {
                    statusCode = 409;
                    mensagem = "O CPF informado já consta em sistema.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }

        [HttpPut("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Atualizar([FromRoute] string cpf, [FromBody] ClienteInputModel clienteUpdate)
        {
            try
            {
                var cliente = await _clientesService.Atualizar(cpf, clienteUpdate);

                return StatusCode(200, cliente);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "O Cpf informado não consta em sistema.";
                }
                else if (ex.Message.Contains("Cliente não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Cliente não encontrado em sistema.";
                }
                else if (ex.Message.Contains("Endereco não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Endereço não encontrado em sistema.";
                }
                else if (ex.Message.Contains("InformacoesContato não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Informações de contato não encontradas em sistema.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> Deletar([FromRoute] string cpf)
        {
            try
            {
                await _clientesService.Deletar(cpf);

                return StatusCode(200, "Cliente deletado com sucesso!");
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("Cliente não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Cliente não consta em sistema.";
                }
                else if (ex.Message.Contains("Endereco não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Endereço não consta em sistema.";
                }
                else if (ex.Message.Contains("Informações de contato não constam em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Informações de contato não constam em sistema.";
                } else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }
    }
}
