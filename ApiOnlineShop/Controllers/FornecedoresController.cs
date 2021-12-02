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
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
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
                    return BadRequest(valido);

                return Ok(await _fornecedoresServices.Atualizar(cnpj, fornecedorUpdate));
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
