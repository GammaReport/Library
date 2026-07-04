namespace GammaReportLibrary.Models.Common;

/// <summary>
/// Wraps typed API data with status and transport metadata.
/// </summary>
/// <typeparam name="T">Typed payload type.</typeparam>
public sealed class GammaApiResult<T>
{
    /// <summary>
    /// Gets or sets HTTP status code returned by the API.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Gets or sets typed payload.
    /// </summary>
    public required T Data { get; init; }

    /// <summary>
    /// Gets or sets API rate-limit metadata, when present.
    /// </summary>
    public RateLimitMetadata? RateLimit { get; init; }
}
