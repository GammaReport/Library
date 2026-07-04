using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Generations;

/// <summary>
/// Represents create-generation response payload.
/// </summary>
public sealed class StartGenerationResponse
{
    /// <summary>
    /// Identifier of the asynchronous generation job.
    /// </summary>
    public required string GenerationId { get; init; }

    /// <summary>
    /// Initial job status when the API includes it.
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Direct Gamma URL when returned by the API.
    /// </summary>
    public string? GammaUrl { get; init; }

    /// <summary>
    /// Informational warnings emitted by Gamma for ignored or normalized parameters.
    /// </summary>
    public string? Warnings { get; init; }

    /// <summary>
    /// Raw response body preserved for forward-compatibility and diagnostics.
    /// </summary>
    public JsonObject? RawBody { get; init; }
}

/// <summary>
/// Represents GET /v1.0/generations/{id} response payload.
/// </summary>
public sealed class GenerationStatusResponse
{
    /// <summary>
    /// Identifier of the asynchronous generation job.
    /// </summary>
    public required string GenerationId { get; init; }

    /// <summary>
    /// Current state of the generation job.
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Gamma file identifier returned once generation completes.
    /// </summary>
    public string? GammaId { get; init; }

    /// <summary>
    /// Direct Gamma URL returned once generation completes.
    /// </summary>
    public string? GammaUrl { get; init; }

    /// <summary>
    /// Export URL returned when an export format was requested.
    /// </summary>
    public string? ExportUrl { get; init; }

    /// <summary>
    /// Credit usage information, when returned by Gamma.
    /// </summary>
    public CreditUsage? Credits { get; init; }

    /// <summary>
    /// Error details when the generation fails.
    /// </summary>
    public GammaApiError? Error { get; init; }

    /// <summary>
    /// Raw response body preserved for forward-compatibility and diagnostics.
    /// </summary>
    public JsonObject? RawBody { get; init; }
}

/// <summary>
/// Credit usage returned by Gamma for a generation.
/// </summary>
public sealed class CreditUsage
{
    /// <summary>
    /// Credits consumed by the generation.
    /// </summary>
    public int? Deducted { get; init; }

    /// <summary>
    /// Remaining credits after the generation.
    /// </summary>
    public int? Remaining { get; init; }
}

/// <summary>
/// Error details returned by Gamma in a failed generation status response.
/// </summary>
public sealed class GammaApiError
{
    /// <summary>
    /// Error message when available.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Full raw error object preserved for diagnostics.
    /// </summary>
    public JsonObject? Details { get; init; }
}
