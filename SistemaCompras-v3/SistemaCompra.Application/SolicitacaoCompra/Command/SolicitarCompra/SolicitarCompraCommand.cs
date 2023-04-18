using MediatR;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System.Collections.Generic;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.SolicitarCompra
{
    public class SolicitarCompraCommand : IRequest<bool>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public IList<Item> Itens { get; set; }
    }
}
