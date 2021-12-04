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
                return Created("Cliente cadastrado com sucesso!", await _clientesService.Inserir(clienteInsert));
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
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

        [HttpPut("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Atualizar([FromRoute] string cpf, [FromBody] ClienteInputModel clienteUpdate)
        {
            try
            {
                return Ok(await _clientesService.Atualizar(cpf, clienteUpdate));
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
