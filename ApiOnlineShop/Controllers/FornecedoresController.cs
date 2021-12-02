using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedoresService _fornecedoresServices;

        public FornecedoresController(IFornecedoresService fornecedoresService)
        {
            _fornecedoresServices = fornecedoresService;
        }

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<FornecedorViewModel>> Obter([FromRoute] string cnpj)
        {
            try
            {
                return Ok(await _fornecedoresServices.Obter(cnpj));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"O seguinte erro ocorreu: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Inserir([FromBody] FornecedorInputModel fornecedorInsert)
        {
            try
            {
                var valido = _fornecedoresServices.ValidarDados(fornecedorInsert);

                if (valido.MensagensDeErro.Count > 0)
                    return BadRequest(valido);
                
                return Created("Fornecedor cadastro com sucesso!", await _fornecedoresServices.Inserir(fornecedorInsert));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"O seguinte erro ocorreu: {e.Message}");
            }
        }

        /*
        [HttpPut("{cnpj}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar([FromRoute] string cnpj, [FromBody] FornecedorInputModel fornecedorUpdate)
        {
            try
            {
                var fornecedor = await _fornecedoresServices.Atualizar(cnpj, fornecedorUpdate);

                return StatusCode(200, fornecedor);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "O CNPJ informado não consta em sistema.";
                }
                else if (ex.Message.Contains("Fornecedor não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Fornecedor não encontrado em sistema.";
                }
                else if (ex.Message.Contains("Endereco não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Endereco não encontrado em sistema.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }
        */

        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> Deletar([FromRoute] string cnpj)
        {
            try
            {
                await _fornecedoresServices.Deletar(cnpj);

                return StatusCode(200, "Fornecedor excluído com sucesso!");
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("Fornecedor não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Fornecedor não consta em sistema.";
                }
                else if (ex.Message.Contains("Endereco não consta em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Endereco não consta em sistema.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }
    }
}
