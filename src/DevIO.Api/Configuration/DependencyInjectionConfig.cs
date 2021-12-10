using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            #region Repositório e Contexto

            services.AddScoped<MeuDbContext>();

            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            #endregion

            #region Notificador e Servicos

            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IFornecedorService, FornecedorService>();

            services.AddScoped<IProdutoService, ProdutoService>();

            #endregion
        }
    }
}