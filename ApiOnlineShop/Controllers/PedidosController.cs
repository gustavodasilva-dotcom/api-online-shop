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

    }
}
