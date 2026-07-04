namespace GammaReportLibrary.Models.Common;

/// <summary>
/// Represents the standard Gamma rate-limit headers.
/// </summary>
public sealed class RateLimitMetadata
{
    /// <summary>
    /// Maximum requests allowed in the current burst window.
    /// </summary>
    public int? LimitBurst { get; init; }

    /// <summary>
    /// Requests remaining in the current burst window.
    /// </summary>
    public int? RemainingBurst { get; init; }

    /// <summary>
    /// Maximum requests allowed in the current hourly window.
    /// </summary>
    public int? LimitHourly { get; init; }

    /// <summary>
    /// Requests remaining in the current hourly window.
    /// </summary>
    public int? RemainingHourly { get; init; }

    /// <summary>
    /// Maximum requests allowed in the current daily window.
    /// </summary>
    public int? LimitDaily { get; init; }

    /// <summary>
    /// Requests remaining in the current daily window.
    /// </summary>
    public int? RemainingDaily { get; init; }
}
