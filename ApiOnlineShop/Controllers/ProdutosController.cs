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
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;

        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [HttpGet("{id:int}/{tipo:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterLista([FromRoute] int id, [FromRoute] int tipo)
        {
            try
            {
                var produtos = await _produtosService.Obter(id, tipo);

                return StatusCode(200, produtos);
            }
            catch (SqlException ex)
            {
                int statusCode;
                string mensagem;

                if (ex.Message.Contains("O tipo retorno informado não está especificado."))
                {
                    statusCode = 404;
                    mensagem = "O tipo retorno informado não está especificado.";
                }
                else if (ex.Message.Contains("O id informado não corresponde a nenhum produto."))
                {
                    statusCode = 404;
                    mensagem = "O id informado não corresponde a nenhum produto.";
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
