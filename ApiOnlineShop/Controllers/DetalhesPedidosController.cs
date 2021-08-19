using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiOnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalhesPedidosController : ControllerBase
    {
        private readonly IDetalhesPedidosService _detalhesPedidosService;

        public DetalhesPedidosController(IDetalhesPedidosService detalhesPedidosService)
        {
            _detalhesPedidosService = detalhesPedidosService;
        }

        [HttpGet("{pedidoId:int}")]
        public async Task<ActionResult<IEnumerable<DetalhesPedidoViewModel>>> Obter([FromRoute] int pedidoId)
        {
            try
            {
                var detalhesPedidos = await _detalhesPedidosService.Obter(pedidoId);

                return StatusCode(200, detalhesPedidos);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("Esse pedido não existe no sistema."))
                {
                    statusCode = 404;
                    mensagem = "Esse pedido não existe no sistema.";
                }
                else if (ex.Message.Contains("Esse pedido não possui itens."))
                {
                    statusCode = 404;
                    mensagem = "Esse pedido não possui itens.";
                }
                else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }

        [HttpPost("{pedidoId:int}")]
        public async Task<ActionResult<DetalhesPedidoViewModel>> Inserir([FromRoute] int pedidoId, [FromBody] DetalhesPedidoInputModel detalhesPedidoInsert)
        {
            try
            {
                var detalhesPedido = await _detalhesPedidosService.Inserir(pedidoId, detalhesPedidoInsert);

                return StatusCode(201, detalhesPedido);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("Pedido não foi encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Pedido não foi encontrado em sistema.";
                }
                else if (ex.Message.Contains("Produto não foi encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Produto não foi encontrado em sistema.";
                }
                else if (ex.Message.Contains("A quantidade não pode ser igual ou menor a 0."))
                {
                    statusCode = 422;
                    mensagem = "A quantidade não pode ser igual ou menor a 0.";
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
