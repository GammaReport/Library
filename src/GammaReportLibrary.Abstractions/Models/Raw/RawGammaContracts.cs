using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Raw;

/// <summary>
/// Low-level request contract for full API access.
/// </summary>
public sealed class RawGammaRequest
{
    /// <summary>
    /// HTTP method used to call Gamma API.
    /// </summary>
    public required string Method { get; init; }

    /// <summary>
    /// Relative API path such as /generations or /themes.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// Optional query-string parameters.
    /// </summary>
    public IReadOnlyDictionary<string, string?>? Query { get; init; }

    /// <summary>
    /// Optional request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string?>? Headers { get; init; }

    /// <summary>
    /// Optional JSON body to send with the request.
    /// </summary>
    public JsonNode? JsonBody { get; init; }
}

/// <summary>
/// Low-level response contract preserving status, headers and body.
/// </summary>
public sealed class RawGammaResponse
{
    /// <summary>
    /// HTTP status code returned by Gamma API.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// All response headers returned by Gamma API.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> Headers { get; init; } =
        new Dictionary<string, IReadOnlyList<string>>();

    /// <summary>
    /// Parsed JSON body when the response contains valid JSON.
    /// </summary>
    public JsonNode? JsonBody { get; init; }

    /// <summary>
    /// Raw textual response body.
    /// </summary>
    public string? RawTextBody { get; init; }
}
