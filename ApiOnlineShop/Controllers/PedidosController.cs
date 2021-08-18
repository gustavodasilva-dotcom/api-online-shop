using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
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
                var pedido = await _pedidosService.Obter(pedidoId);

                return StatusCode(200, pedido);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("O pedido não foi encontrado na base de dados."))
                {
                    statusCode = 404;
                    mensagem = "O pedido não foi encontrado na base de dados.";
                } else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }   
        }

        [HttpPost]
        public async Task<ActionResult<PedidoViewModel>> Inserir([FromBody] PedidoInputModel pedidoInsert)
        {
            try
            {
                var pedido = await _pedidosService.InserirCabecalho(pedidoInsert);

                return StatusCode(201, pedido);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("O id do cliente não pode ser nulo ou zero."))
                {
                    statusCode = 422;
                    mensagem = "O id do cliente não pode ser nulo ou zero.";
                }
                else if (ex.Message.Contains("Cliente não encontrado em sistema."))
                {
                    statusCode = 404;
                    mensagem = "Cliente não encontrado em sistema.";
                }
                else if (ex.Message.Contains("A data da compra não pode ser maior que a data de hoje."))
                {
                    statusCode = 422;
                    mensagem = "A data da compra não pode ser maior que a data de hoje.";
                } else
                {
                    statusCode = 500;
                    mensagem = "Ops! Ocorreu um erro no servidor. Por gentileza, tentar novamente.";
                }

                return StatusCode(statusCode, mensagem);
            }
        }

    }
}
