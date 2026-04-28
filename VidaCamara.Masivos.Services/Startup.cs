using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.LoginModule;
using VidaCamara.Domain.Services.Repositorios.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Domain.Services.Repositorios.Helper;
using VidaCamara.Infrastructure.Connection;
using VidaCamara.Infrastructure.Data.Apeseg;
using VidaCamara.Infrastructure.Data.Configurar;
using VidaCamara.Infrastructure.Data.Helper;
using VidaCamara.Infrastructure.Data.LoginModule;
using VidaCamara.Masivos.Services.Extensions;
using VidaCamara.Masivos.Services.Services.Apeseg;
using VidaCamara.Masivos.Services.Services.Configurar.RolModule;
using VidaCamara.Masivos.Services.Services.Configurar.TipoPerfilModule;
using VidaCamara.Masivos.Services.Services.Configurar.UsuarioModule;
using VidaCamara.Masivos.Services.Services.Helper;
using VidaCamara.Masivos.Services.Services.LoginModule;


namespace VidaCamara.Masivos.Services
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        readonly string MyAllowAnyOrigin = "_AllowAnyOrigin";

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            // loggerFactory.ConfigureNLog(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureLoggerService();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowAnyOrigin,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader() //.AllowCredentials()
                    );
            });

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup).Assembly);
            //services.AddAutoMapper();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            if (appSettingsSection.Get<AppSettings>().Activo == 0)
            {
                appSettingsSection = Configuration.GetSection("AppSettingsDev");
            }

            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            RegisterAplicacionesServices(services);
            RegisterRepositoryServices(services);

            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();

                options.OperationFilter<VidaCamara.Masivos.Services.Extensions.AuthorizationHeaderFilter>();

                options.IncludeXmlComments(Path.ChangeExtension(typeof(Startup).Assembly.Location, "xml"));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Configuration["App:Title"],
                    Version = Configuration["App:Version"],
                    Description = Configuration["App:Description"]
                }
                );
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

                options.CustomSchemaIds(x => x.FullName);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });*/
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowAnyOrigin);
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plataforma API v1");
                    // Esta línea es la clave: establece Swagger en la raíz (https://localhost:44343/)
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Plataforma API");
                    c.RoutePrefix = string.Empty;
                });
            }
            //.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Plataforma API");
            //    c.InjectStylesheet("../css/swagger.min.css");
            //});
        }

        private static void RegisterAplicacionesServices(IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ITipoPerfilService, TipoPerfilService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IHelperService, HelperService>();
            services.AddScoped<IApesegService, ApesegService>();
            services.AddScoped<IMaestraService, MaestraService>();

        }
        private static void RegisterRepositoryServices(IServiceCollection services)
        {
            services.AddScoped<IConnectionBase, ConnectionBase>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ITipoPerfilRepository, TipoPerfilRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IHelperRepository, HelperRepository>();
            services.AddScoped<IApesegRepository, ApesegRepository>();
            services.AddScoped<IMaestraRepository, MaestraRepository>(); ;
        }
    }
}
