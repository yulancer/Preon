using AutoMapper;
using Preon.Solver.Contracts.Abstractions;
using Preon.Solver.Integration.Mapper;
using Preon.Solver.Integration.Options;
using Preon.Solver.Integration.Services;

namespace Preon.Solver.Extensions;

public static class RegisterClientExtension
{
    public static IServiceCollection RegisterApiClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            return services
                .RegisterBanApiClient(
                    configuration, 
                    nameof(WebApiClientOptions))
                .RegisterApiClientOptions(
                    configuration,
                    nameof(WebApiClientOptions));
        }
        private static IServiceCollection RegisterApiClientOptions(
            this IServiceCollection services,
            IConfiguration configuration,
            string optionsSectionPath)
        {
            var options = configuration
                .GetSection(optionsSectionPath)
                .Get<WebApiClientOptions>();

            if (options is null)
            {
                throw new InvalidOperationException(nameof(WebApiClientOptions));
            }

            return services.AddSingleton(options);
        }

        private static IServiceCollection RegisterBanApiClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string optionsSectionPath)
        {
            var options = configuration
                .GetSection(optionsSectionPath)
                .Get<WebApiClientOptions>();
            
            services
                .AddHttpClient<IWebApiClient, WebApiClient>(o =>
                {
                    o.BaseAddress = new Uri(options.BaseUrl);
                    o.Timeout = TimeSpan.FromSeconds(options.TimeOutSeconds);
                });
            
            return services;
        }
}