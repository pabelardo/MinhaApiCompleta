using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings;

public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(p => p.ID);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)")
            .HasColumnName("NOME");

        builder.Property(p => p.Documento)
            .IsRequired()
            .HasColumnType("varchar(14)")
            .HasColumnName("DOCUMENTO");

        builder.Property(p => p.TipoFornecedor)
            .HasColumnName("TIPO_FORNECEDOR");

        builder.Property(p => p.Ativo)
            .HasColumnName("ATIVO");

        // 1 : 1 => Fornecedor : Endereco
        builder.HasOne(f => f.Endereco)
            .WithOne(e => e.Fornecedor);

        // 1 : N => Fornecedor : Produtos
        builder.HasMany(f => f.Produtos)
            .WithOne(p => p.Fornecedor)
            .HasForeignKey(p => p.FornecedorId);

        builder.ToTable("FORNECEDORES");
    }
}