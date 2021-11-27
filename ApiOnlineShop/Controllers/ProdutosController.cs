using ApiOnlineShop.CustomExceptions;
using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet("{categoriaId:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> Obter([FromRoute] int categoriaId)
        {
            try
            {
                return Ok(await _produtosService.Obter(categoriaId));
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

        [HttpGet("{produtoId:int}/{categoriaId:int}")]
        public async Task<ActionResult<ProdutoViewModel>> Obter([FromRoute] int produtoId, [FromRoute] int categoriaId)
        {
            try
            {
                return Ok(await _produtosService.Obter(produtoId, categoriaId));
            }
            catch (UnprocessableEntityException e)
            {
                return UnprocessableEntity(e.Message);
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
        public async Task<ActionResult> Inserir([FromBody] ProdutoInputModel produto)
        {
            try
            {
                var produtoId = await _produtosService.Inserir(produto);

                return Created($"Produto criado com sucesso no id {produtoId}.", _produtosService.Obter(produtoId, produto.CategoriaId));
            }
            catch (UnprocessableEntityException e)
            {
                return UnprocessableEntity(e.Message);
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
