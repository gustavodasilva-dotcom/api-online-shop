using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Services.Interfaces;

namespace ApiOnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly ILogsService _logsService;

        private readonly IFornecedoresService _fornecedoresServices;

        public FornecedoresController(ILogsService logsService, IFornecedoresService fornecedoresService)
        {
            _logsService = logsService;

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
                {
                    await _logsService.GravarLog(fornecedorInsert, valido, "JSON de entrada possui erros.", false);

                    return BadRequest(valido);
                }

                var fornecedor = await _fornecedoresServices.Inserir(fornecedorInsert);

                await _logsService.GravarLog(fornecedorInsert, fornecedor, "Fornecedor cadastrado com sucesso!", fornecedor.FornecedorId, false);

                return Created("Fornecedor cadastro com sucesso!", fornecedor);
            }
            catch (NotFoundException e)
            {
                await _logsService.GravarLog(fornecedorInsert, e.Message, false);

                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                await _logsService.GravarLog(fornecedorInsert, e.Message, false);

                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                await _logsService.GravarLog(fornecedorInsert, e.Message, false);

                return StatusCode(500, $"O seguinte erro ocorreu: {e.Message}");
            }
        }

        [HttpPut("{cnpj}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar([FromRoute] string cnpj, [FromBody] FornecedorInputModel fornecedorUpdate)
        {
            try
            {
                var valido = _fornecedoresServices.ValidarDados(cnpj, fornecedorUpdate);

                if (valido.MensagensDeErro.Count > 0)
                {
                    await _logsService.GravarLog(fornecedorUpdate, valido, "JSON de entrada possui erros.", false);

                    return BadRequest(valido);
                }

                var fornecedor = await _fornecedoresServices.Atualizar(cnpj, fornecedorUpdate);

                await _logsService.GravarLog(fornecedorUpdate, fornecedor, "Fornecedor atualizado com sucesso!", fornecedor.FornecedorId, false);

                return Ok(fornecedor);
            }
            catch (NotFoundException e)
            {
                await _logsService.GravarLog(fornecedorUpdate, e.Message, false);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                await _logsService.GravarLog(fornecedorUpdate, e.Message, false);

                return StatusCode(500, $"O seguinte erro ocorreu: {e.Message}");
            }
        }

        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> Deletar([FromRoute] string cnpj)
        {
            try
            {
                await _fornecedoresServices.Deletar(cnpj);

                return Ok($"Fornecedor {cnpj} excluído com sucesso!");
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
    }
}
