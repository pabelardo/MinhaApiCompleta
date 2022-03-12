﻿// <auto-generated />
using System;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevIO.Data.Migrations
{
    [DbContext(typeof(MeuDbContext))]
    [Migration("20220205055849_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevIO.Business.Models.Endereco", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("BAIRRO");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("CEP");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("CIDADE");

                    b.Property<string>("Complemento")
                        .HasColumnType("varchar(250)")
                        .HasColumnName("COMPLEMENTO");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("ESTADO");

                    b.Property<Guid>("FornecedorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("FORNECEDOR_ID");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("LOGRADOURO");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NUMERO");

                    b.HasKey("ID");

                    b.HasIndex("FornecedorId")
                        .IsUnique();

                    b.ToTable("ENDERECOS", (string)null);
                });

            modelBuilder.Entity("DevIO.Business.Models.Fornecedor", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("ATIVO");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("varchar(14)")
                        .HasColumnName("DOCUMENTO");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("NOME");

                    b.Property<int>("TipoFornecedor")
                        .HasColumnType("int")
                        .HasColumnName("TIPO_FORNECEDOR");

                    b.HasKey("ID");

                    b.ToTable("FORNECEDORES", (string)null);
                });

            modelBuilder.Entity("DevIO.Business.Models.Produto", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("ATIVO");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2")
                        .HasColumnName("DATA_CADASTRO");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("DESCRICAO");

                    b.Property<Guid>("FornecedorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("FORNECEDOR_ID");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("IMAGEM");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("NOME");

                    b.Property<decimal>("Valor")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("VALOR");

                    b.HasKey("ID");

                    b.HasIndex("FornecedorId");

                    b.ToTable("PRODUTOS", (string)null);
                });

            modelBuilder.Entity("DevIO.Business.Models.Endereco", b =>
                {
                    b.HasOne("DevIO.Business.Models.Fornecedor", "Fornecedor")
                        .WithOne("Endereco")
                        .HasForeignKey("DevIO.Business.Models.Endereco", "FornecedorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("DevIO.Business.Models.Produto", b =>
                {
                    b.HasOne("DevIO.Business.Models.Fornecedor", "Fornecedor")
                        .WithMany("Produtos")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("DevIO.Business.Models.Fornecedor", b =>
                {
                    b.Navigation("Endereco");

                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
