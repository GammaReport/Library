using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Workspace;

/// <summary>
/// Query for listing themes.
/// </summary>
public sealed class ThemeQuery
{
    /// <summary>
    /// Page size to request.
    /// </summary>
    public int? Limit { get; init; }

    /// <summary>
    /// Cursor returned by a previous page.
    /// </summary>
    public string? After { get; init; }

    /// <summary>
    /// Name-based filter.
    /// </summary>
    public string? Query { get; init; }

    /// <summary>
    /// Optional theme type filter, such as standard or custom.
    /// </summary>
    public string? Type { get; init; }
}

/// <summary>
/// List themes response.
/// </summary>
public sealed class ThemeListResponse
{
    /// <summary>
    /// Theme items returned by the API.
    /// </summary>
    public IReadOnlyList<ThemeItem> Data { get; init; } = [];

    /// <summary>
    /// Indicates whether additional pages exist.
    /// </summary>
    public bool HasMore { get; init; }

    /// <summary>
    /// Cursor for the next page when more data exists.
    /// </summary>
    public string? NextCursor { get; init; }

    /// <summary>
    /// Raw response body preserved for forward-compatibility and diagnostics.
    /// </summary>
    public JsonObject? RawBody { get; init; }
}

/// <summary>
/// Single theme item returned by Gamma.
/// </summary>
public sealed class ThemeItem
{
    /// <summary>
    /// Theme identifier.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the theme.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Theme type, such as standard or custom.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Color keywords describing the theme palette.
    /// </summary>
    public IReadOnlyList<string>? ColorKeywords { get; init; }

    /// <summary>
    /// Tone keywords describing the visual style.
    /// </summary>
    public IReadOnlyList<string>? ToneKeywords { get; init; }
}
