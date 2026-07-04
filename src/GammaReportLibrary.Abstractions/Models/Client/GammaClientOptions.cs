namespace GammaReportLibrary.Models.Client;

/// <summary>
/// Configures SDK-level behavior and connectivity settings.
/// </summary>
public sealed class GammaClientOptions
{
    /// <summary>
    /// Base URL for Gamma Public API.
    /// </summary>
    public string BaseUrl { get; set; } = "https://public-api.gamma.app/v1.0";

    /// <summary>
    /// API key sent in X-API-KEY header.
    /// </summary>
    public required string ApiKey { get; set; }

    /// <summary>
    /// Default polling interval for wait operations.
    /// </summary>
    public TimeSpan DefaultPollingInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Default timeout for wait operations.
    /// </summary>
    public TimeSpan DefaultWaitTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Number of retry attempts for transient HTTP failures.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 2;

    /// <summary>
    /// Base delay used for exponential backoff retries.
    /// </summary>
    public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Upper bound for retry delay.
    /// </summary>
    public TimeSpan RetryMaxDelay { get; set; } = TimeSpan.FromSeconds(30);
}
