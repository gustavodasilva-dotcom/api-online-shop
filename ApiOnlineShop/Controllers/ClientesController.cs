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
            // TODO: Criar Exception especial para clientes não encontrados.
            catch (Exception ex)
            {
                return StatusCode(204, "Cliente não foi encontrado encontrado.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteViewModel>> Inserir([FromBody] ClienteInputModel clienteInsert)
        {
            var cliente = await _clientesService.Inserir(clienteInsert);

            return StatusCode(200, cliente);
        }
    }
}
