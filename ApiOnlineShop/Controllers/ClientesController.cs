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
    public class ClientesController : ControllerBase
    {
        private readonly ILogsService _logsService;

        private readonly IClientesService _clientesService;

        public ClientesController(ILogsService logsService, IClientesService clientesService)
        {
            _logsService = logsService;

            _clientesService = clientesService;
        }

        [HttpGet("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Obter([FromRoute] string cpf)
        {
            try
            {
                return Ok(await _clientesService.Obter(cpf));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteViewModel>> Inserir([FromBody] ClienteInputModel clienteInsert)
        {
            try
            {
                var valido = _clientesService.ValidarDados(clienteInsert);

                if (valido.MensagensDeErro.Count > 0)
                {
                    await _logsService.GravarLog(clienteInsert, valido, "JSON de entrada possui erros.", false);

                    return BadRequest(valido);
                }

                var cliente = await _clientesService.Inserir(clienteInsert);

                await _logsService.GravarLog(clienteInsert, cliente, "Cliente cadastrado com sucesso!", cliente.ClienteId, false);

                return Created("Cliente cadastrado com sucesso!", cliente);
            }
            catch (ConflictException e)
            {
                await _logsService.GravarLog(clienteInsert, e.Message, false);

                return Conflict(e.Message);
            }
            catch (NotFoundException e)
            {
                await _logsService.GravarLog(clienteInsert, e.Message, false);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                await _logsService.GravarLog(clienteInsert, e.Message, false);

                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Atualizar([FromRoute] string cpf, [FromBody] ClienteInputModel clienteUpdate)
        {
            try
            {
                var valido = _clientesService.ValidarDados(cpf, clienteUpdate);

                if (valido.MensagensDeErro.Count > 0)
                {
                    await _logsService.GravarLog(clienteUpdate, valido, "JSON de entrada possui erros.", false);

                    return BadRequest(valido);
                }

                var cliente = await _clientesService.Atualizar(cpf, clienteUpdate);

                await _logsService.GravarLog(clienteUpdate, cliente, "Cliente atualizado com sucesso!", cliente.ClienteId, false);

                return Ok(cliente);
            }
            catch (NotFoundException e)
            {
                await _logsService.GravarLog(clienteUpdate, e.Message, false);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                await _logsService.GravarLog(clienteUpdate, e.Message, false);

                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> Deletar([FromRoute] string cpf)
        {
            try
            {
                await _clientesService.Deletar(cpf);

                return Ok("Cliente deletado com sucesso!");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
