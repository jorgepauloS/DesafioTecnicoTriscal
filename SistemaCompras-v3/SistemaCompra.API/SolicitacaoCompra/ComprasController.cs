using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaCompra.Application.SolicitacaoCompra.Command.SolicitarCompra;
using SistemaCompra.Domain.Core;
using System;
using System.Threading.Tasks;

namespace SistemaCompra.API.SolicitacaoCompra
{
    public class ComprasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComprasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("compras/solicitacao")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SolicitarCompra([FromBody] SolicitarCompraCommand solicitarCompraCommand)
        {
            try
            {
                if (await _mediator.Send(solicitarCompraCommand))
                    return StatusCode(201);
                else
                    return BadRequest();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }
    }
}
