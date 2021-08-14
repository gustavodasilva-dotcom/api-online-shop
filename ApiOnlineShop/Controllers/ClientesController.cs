using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
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
            catch (Exception ex)
            {
                return StatusCode(204, "Cliente não foi encontrado encontrado.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteViewModel>> Inserir([FromBody] ClienteInputModel clienteInsert)
        {
            //try
            //{
                var cliente = await _clientesService.Inserir(clienteInsert);

                return StatusCode(200, cliente);
            //}
            // TODO: Criar Exception especial para as validações da rotina.
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "Erro interno no servidor! Por favor, tentar novamente.");
            //}
        }

        [HttpPut("{cpf}")]
        public async Task<ActionResult<ClienteViewModel>> Atualizar([FromRoute] string cpf, [FromBody] ClienteInputModel clienteUpdate)
        {
            try
            {
                var cliente = await _clientesService.Atualizar(cpf, clienteUpdate);

                return StatusCode(200, cliente);
            }
            // TODO: Criar Exception especial para as validações da rotina.
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor! Por favor, tentar novamente.");
            }
        }

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> Deletar([FromRoute] string cpf)
        {
            await _clientesService.Deletar(cpf);

            return StatusCode(200, "Cliente deletado com sucesso!");
        }
    }
}
