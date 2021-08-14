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
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedoresService _fornecedoresServices;

        public FornecedoresController(IFornecedoresService fornecedoresService)
        {
            _fornecedoresServices = fornecedoresService;
        }

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<FornecedorViewModel>> Obter([FromRoute] string cnpj)
        {
            try
            {
                var fornecedor = await _fornecedoresServices.Obter(cnpj);

                return StatusCode(200, fornecedor);
            }
            // TODO: Criar Exception especial para as validações da rotina.
            catch (Exception ex)
            {
                return StatusCode(204, "Fornecedor não encontrado no sistema!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Inserir([FromBody] FornecedorInputModel fornecedorInsert)
        {
            try
            {
                var fornecedor = await _fornecedoresServices.Inserir(fornecedorInsert);

                return StatusCode(200, fornecedor);
            }
            // TODO: Criar Exception especial para as validações da rotina.
            catch (Exception ex)
            {
                return StatusCode(204, "Fornecedor já está cadastrado no sistema!");
            }
        }

        [HttpPut("{cnpj}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar([FromRoute] string cnpj, [FromBody] FornecedorInputModel fornecedorUpdate)
        {
            try
            {
                var fornecedor = await _fornecedoresServices.Atualizar(cnpj, fornecedorUpdate);

                return StatusCode(200, fornecedor);
            }
            // TODO: Criar Exception especial para as validações da rotina.
            catch (Exception ex)
            {
                return StatusCode(204, "Fornecedor não consta em sistema.");
            }
        }

        [HttpDelete("{cnpj}")]
        public async Task<ActionResult> Deletar([FromRoute] string cnpj)
        {
            try
            {
                await _fornecedoresServices.Deletar(cnpj);

                return StatusCode(200, "Fornecedor excluído com sucesso!");
            }
            // TODO: Criar Exception especial para as validações da rotina.
            catch (Exception ex)
            {
                return StatusCode(204, "Fornecedor não consta em sistema.");
            }
        }
    }
}
