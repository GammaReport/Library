using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Workspace;

/// <summary>
/// Query for listing folders.
/// </summary>
public sealed class FolderQuery
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
}

/// <summary>
/// List folders response.
/// </summary>
public sealed class FolderListResponse
{
    /// <summary>
    /// Folder items returned by the API.
    /// </summary>
    public IReadOnlyList<FolderItem> Data { get; init; } = [];

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
/// Single folder item returned by Gamma.
/// </summary>
public sealed class FolderItem
{
    /// <summary>
    /// Folder identifier.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the folder.
    /// </summary>
    public required string Name { get; init; }
}
