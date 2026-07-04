using GammaReportLibrary.Models.Client;
using Microsoft.Extensions.DependencyInjection;

namespace GammaReportLibrary.DependencyInjection;

/// <summary>
/// Public facade entry point for registering GammaReportLibrary services.
/// </summary>
public static class GammaReportLibraryServiceCollectionExtensions
{
    /// <summary>
    /// Registers GammaReportLibrary typed and raw clients.
    /// </summary>
    /// <param name="services">Target service collection.</param>
    /// <param name="configure">SDK options configuration.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddGammaReportLibrary(
        this IServiceCollection services,
        Action<GammaClientOptions> configure)
    {
        return GammaReportLibrary.Infrastructure.DependencyInjection.GammaServiceCollectionExtensions
            .AddGammaReportLibrary(services, configure);
    }
}
