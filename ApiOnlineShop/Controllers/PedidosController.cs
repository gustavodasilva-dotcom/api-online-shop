using ApiOnlineShop.CustomExceptions;
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
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosService _pedidosService;

        public PedidosController(IPedidosService pedidosService)
        {
            _pedidosService = pedidosService;
        }

        [HttpGet("{pedidoId:int}")]
        public async Task<ActionResult<PedidoViewModel>> Obter([FromRoute] int pedidoId)
        {
            try
            {
                return Ok(await _pedidosService.Obter(pedidoId));
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
        public async Task<ActionResult<PedidoViewModel>> Inserir([FromBody] PedidoInputModel pedidoInsert)
        {
            try
            {
                var pedidoID = await _pedidosService.Inserir(pedidoInsert);

                return Created($"Pedido criado com sucesso no id {pedidoID}.", await _pedidosService.Obter(pedidoID));
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
