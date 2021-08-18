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
    }
}
