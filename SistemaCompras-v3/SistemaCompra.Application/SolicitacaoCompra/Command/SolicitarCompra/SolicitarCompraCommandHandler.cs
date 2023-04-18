using MediatR;
using SistemaCompra.Infra.Data.UoW;
using System.Threading.Tasks;
using System.Threading;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.Core;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.SolicitarCompra
{
    public class SolicitarCompraCommandHandler : CommandHandler, IRequestHandler<SolicitarCompraCommand, bool>
    {
        private readonly SolicitacaoCompraAgg.ISolicitacaoCompraRepository solicitacaoCompraRepository;
        private readonly ProdutoAgg.IProdutoRepository produtoRepository;

        public SolicitarCompraCommandHandler(SolicitacaoCompraAgg.ISolicitacaoCompraRepository solicitacaoCompraRepository, 
            ProdutoAgg.IProdutoRepository produtoRepository, IUnitOfWork uow, IMediator mediator) : base(uow, mediator)
        {
            this.solicitacaoCompraRepository = solicitacaoCompraRepository;
            this.produtoRepository = produtoRepository;
        }

        public Task<bool> Handle(SolicitarCompraCommand request, CancellationToken cancellationToken)
        {
            var solicitacao = new SolicitacaoCompraAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor);

            foreach (SolicitacaoCompraAgg.Item item in request.Itens)
            {
                if (item.Qtde < 1) throw new BusinessRuleException($"Quantidade de produtos inválida: \"{item.Qtde}\".");

                var produto = produtoRepository.Obter(item.Produto.Id);
                if (produto is null) throw new BusinessRuleException($"Id \"{item.Produto.Id}\" de produto inválido.");
                item.Produto = produto;
            }

            solicitacao.RegistrarCompra(request.Itens);
            solicitacaoCompraRepository.RegistrarCompra(solicitacao);

            if (Commit())
            {
                PublishEvents(solicitacao.Events);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
