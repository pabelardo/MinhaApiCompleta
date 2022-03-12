using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings;

public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.HasKey(p => p.ID);

        builder.Property(p => p.FornecedorId)
            .IsRequired()
            .HasColumnName("FORNECEDOR_ID");

        builder.Property(c => c.Logradouro)
            .IsRequired()
            .HasColumnType("varchar(200)")
            .HasColumnName("LOGRADOURO");

        builder.Property(c => c.Numero)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasColumnName("NUMERO");

        builder.Property(c => c.Cep)
            .IsRequired()
            .HasColumnType("varchar(8)")
            .HasColumnName("CEP");

        builder.Property(c => c.Complemento)
            .HasColumnType("varchar(250)")
            .HasColumnName("COMPLEMENTO");

        builder.Property(c => c.Bairro)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasColumnName("BAIRRO");

        builder.Property(c => c.Cidade)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasColumnName("CIDADE");

        builder.Property(c => c.Estado)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasColumnName("ESTADO");

        builder.ToTable("ENDERECOS");
    }
}