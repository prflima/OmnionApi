using Microsoft.Extensions.DependencyInjection;
using Omnion.Business.Interfaces;
using Omnion.Business.Notificacoes;
using Omnion.Business.Services;
using Omnion.Repository.Interfaces;
using Omnion.Repository.Repositories;

namespace OmnionAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDepedencies(this IServiceCollection services)
        {

            #region Injeção Business
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IClienteService, ClienteService>();
            #endregion

            #region Injeção Repository
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<ITelefoneRepository, TelefoneRepository>();
            services.AddScoped<IContaRepository, ContaRepository>();
            #endregion
            return services;
        }
    }
}
