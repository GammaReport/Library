using GammaReportLibrary.Application;
using GammaReportLibrary.Abstractions;
using GammaReportLibrary.Models.Client;
using Microsoft.Extensions.DependencyInjection;

namespace GammaReportLibrary.Infrastructure.DependencyInjection;

public static class GammaServiceCollectionExtensions
{
    public static IServiceCollection AddGammaReportLibrary(
        this IServiceCollection services,
        Action<GammaClientOptions> configure)
    {
        services.AddOptions<GammaClientOptions>().Configure(configure);

        services.AddHttpClient<IGammaRawClient, GammaRawClient>((serviceProvider, client) =>
        {
            var options = serviceProvider
                .GetRequiredService<Microsoft.Extensions.Options.IOptions<GammaClientOptions>>()
                .Value;

            var normalizedBaseUrl = options.BaseUrl.EndsWith("/", StringComparison.Ordinal)
                ? options.BaseUrl
                : options.BaseUrl + "/";

            if (Uri.TryCreate(normalizedBaseUrl, UriKind.Absolute, out var baseUri))
            {
                client.BaseAddress = baseUri;
            }
        });

        services.AddTransient<IGammaClient, GammaClientApplicationService>();

        return services;
    }
}
