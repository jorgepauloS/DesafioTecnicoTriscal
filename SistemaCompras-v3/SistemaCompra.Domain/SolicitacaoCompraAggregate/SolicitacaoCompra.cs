using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; } = new List<Item>();
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; } = new Money();
        public Situacao Situacao { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            var item = new Item(produto, qtde);
            Itens.Add(item);
            TotalGeral = TotalGeral.Add(item.Subtotal);
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
            if (!itens.Any()) throw new BusinessRuleException("A solicitação de compra deve possuir itens!");

            foreach (Item item in itens)
            {
                AdicionarItem(item.Produto, item.Qtde);
            }

            AtualizarCondicaoPagamento();

            AddEvent(new CompraRegistradaEvent(Id, itens, TotalGeral.Value));
        }

        private void AtualizarCondicaoPagamento()
        {
            int condicao = 0;

            if (TotalGeral.Value > 50000)
            {
                condicao = 30;
            }

            CondicaoPagamento = new CondicaoPagamento(condicao);
        }
    }
}
