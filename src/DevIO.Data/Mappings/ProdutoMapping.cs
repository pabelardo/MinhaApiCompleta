using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(p => p.ID);

        builder.Property(p => p.FornecedorId)
            .IsRequired()
            .HasColumnName("FORNECEDOR_ID");

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)")
            .HasColumnName("NOME");

        builder.Property(p => p.Descricao)
            .IsRequired()
            .HasColumnType("varchar(1000)")
            .HasColumnName("DESCRICAO");

        builder.Property(p => p.Imagem)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasColumnName("IMAGEM");

        builder.Property(p => p.Valor)
            .IsRequired()
            .HasColumnName("VALOR")
            .HasPrecision(18,2);

        builder.Property(p => p.Ativo)
            .HasColumnName("ATIVO");

        builder.Property(p => p.DataCadastro)
            .HasColumnName("DATA_CADASTRO");

        builder.ToTable("PRODUTOS");
    }
}