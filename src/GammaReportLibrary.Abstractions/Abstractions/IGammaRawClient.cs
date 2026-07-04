using GammaReportLibrary.Models.Raw;
using GammaReportLibrary.Models.Common;

namespace GammaReportLibrary.Abstractions;

/// <summary>
/// Provides low-level access to Gamma API for complete request/response control.
/// </summary>
public interface IGammaRawClient
{
    /// <summary>
    /// Sends a raw request to Gamma API and returns the raw response envelope.
    /// </summary>
    /// <param name="request">Low-level request contract with method, path, headers, query and JSON body.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The raw response plus status code and rate-limit metadata.</returns>
    Task<GammaApiResult<RawGammaResponse>> SendAsync(
        RawGammaRequest request,
        CancellationToken cancellationToken = default);
}
