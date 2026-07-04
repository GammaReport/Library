using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Gammas;

/// <summary>
/// Archive Gamma response payload.
/// </summary>
public sealed class ArchiveGammaResponse
{
    /// <summary>
    /// Gamma file identifier affected by the archive operation.
    /// </summary>
    public required string GammaId { get; init; }

    /// <summary>
    /// Indicates whether the Gamma is archived after the operation.
    /// </summary>
    public bool Archived { get; init; }

    /// <summary>
    /// Raw response body preserved for forward-compatibility and diagnostics.
    /// </summary>
    public JsonObject? RawBody { get; init; }
}

/// <summary>
/// Delete Gamma response payload.
/// </summary>
public sealed class DeleteGammaResponse
{
    /// <summary>
    /// Operation status returned by Gamma, typically deleted.
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Gamma file identifier affected by the delete operation.
    /// </summary>
    public required string GammaId { get; init; }

    /// <summary>
    /// Human-readable confirmation message.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Raw response body preserved for forward-compatibility and diagnostics.
    /// </summary>
    public JsonObject? RawBody { get; init; }
}
