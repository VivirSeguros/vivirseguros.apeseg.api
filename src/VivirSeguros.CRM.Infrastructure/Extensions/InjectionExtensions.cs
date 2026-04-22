using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using VivirSeguros.CRM.Infrastructure.Persistences.Repositories;
using VivirSeguros.CRM.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection;

namespace VivirSeguros.CRM.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IHelperRepository, HelperRepository>();
            services.AddScoped<IAVirtualRepository, AVirtualRepository>();
            services.AddScoped<IValidaCuentaRepository, ValidaCuentaRepository>();
            //services.AddScoped<INotify, Notify>();

            /*var rabbitMqSection = configuration.GetSection("RabbitMq");
            var exchangeSection = configuration.GetSection("RabbitMqExchange");

            services.AddRabbitMqProducingClientTransient(rabbitMqSection)
                .AddProductionExchange("exchange.name", exchangeSection);*/

            return services;
        }
    }
}
