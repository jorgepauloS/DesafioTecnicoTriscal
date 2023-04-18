using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data.SolicitacaoCompra
{
    public class SolicitacaoCompraConfiguration : IEntityTypeConfiguration<SolicitacaoCompraAgg.SolicitacaoCompra>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraAgg.SolicitacaoCompra> builder)
        {
            builder.ToTable("SolicitacaoCompra");
            builder.OwnsOne(c => c.TotalGeral).Property("Value").HasColumnName("TotalGeral");
            builder.OwnsOne(c => c.CondicaoPagamento).Property("Valor").HasColumnName("CondicaoPagamento");
            builder.OwnsOne(c => c.NomeFornecedor).Property("Nome").HasColumnName("NomeFornecedor");
            builder.OwnsOne(c => c.UsuarioSolicitante).Property("Nome").HasColumnName("UsuarioSolicitante");
        }
    }
}
