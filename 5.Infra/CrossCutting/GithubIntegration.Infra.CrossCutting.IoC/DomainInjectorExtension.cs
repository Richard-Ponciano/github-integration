using GithubIntegration.AppService;
using GithubIntegration.AppService.Contract.Interface;
using GithubIntegration.Infra.Data;
using GithubIntegration.Infra.Data.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO.Compression;

namespace GithubIntegration.Infra.CrossCutting
{
    public static class DomainInjectorExtension
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, OpenApiInfo apiInfo)
        {
            apiInfo.Contact = new OpenApiContact
            {
                Name = "Richard Ponciano",
                Email = "richard@richardponciano.com.br",
            };
            apiInfo.License = new OpenApiLicense
            {
                Name = "Richard Poniano Leite Me",
            };

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", apiInfo); });

            return services;
        }

        public static IServiceCollection AddApiCompress(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            return services;
        }

        public static IServiceCollection AddAppService(this IServiceCollection services)
        {
            services.AddScoped<IGitRepositorioAppService, GitRepositorioAppService>();

            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IFavoritoRepository, FavoritoRepository>();

            return services;
        }
    }
}