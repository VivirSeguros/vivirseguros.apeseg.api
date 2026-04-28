using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Log.LoggerService;

namespace VidaCamara.Masivos.Services.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

    }
}
