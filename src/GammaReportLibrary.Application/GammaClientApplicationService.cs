using System.Text.Json;
using System.Text.Json.Nodes;
using GammaReportLibrary.Errors;
using Microsoft.Extensions.Options;
using GammaReportLibrary.Models.Raw;
using GammaReportLibrary.Abstractions;
using GammaReportLibrary.Models.Client;
using GammaReportLibrary.Models.Common;
using GammaReportLibrary.Models.Gammas;
using GammaReportLibrary.Models.Workspace;
using GammaReportLibrary.Models.Generations;

namespace GammaReportLibrary.Application;

/// <summary>
/// Implements typed Gamma use cases while staying transport-agnostic via IGammaRawClient.
/// </summary>
public sealed class GammaClientApplicationService : IGammaClient
{
    private readonly IGammaRawClient _rawClient;
    private readonly GammaClientOptions _options;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public GammaClientApplicationService(IGammaRawClient rawClient, IOptions<GammaClientOptions> options)
    {
        _rawClient = rawClient;
        _options = options.Value;
    }

    public async Task<GammaApiResult<StartGenerationResponse>> CreateGenerationAsync(
        CreateGenerationRequest request,
        CancellationToken cancellationToken = default)
    {
        var body = SerializeWithExtensionBag(request, request.AdditionalParameters);

        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "POST",
                Path = "/generations",
                JsonBody = body
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var response = new StartGenerationResponse
        {
            GenerationId = payload?["generationId"]?.GetValue<string>() ?? string.Empty,
            Status = payload?["status"]?.GetValue<string>(),
            GammaUrl = payload?["gammaUrl"]?.GetValue<string>(),
            Warnings = payload?["warnings"]?.GetValue<string>(),
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<StartGenerationResponse>> CreateFromTemplateAsync(
        CreateFromTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        var body = SerializeWithExtensionBag(request, request.AdditionalParameters);

        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "POST",
                Path = "/generations/from-template",
                JsonBody = body
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var response = new StartGenerationResponse
        {
            GenerationId = payload?["generationId"]?.GetValue<string>() ?? string.Empty,
            Status = payload?["status"]?.GetValue<string>(),
            GammaUrl = payload?["gammaUrl"]?.GetValue<string>(),
            Warnings = payload?["warnings"]?.GetValue<string>(),
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<GenerationStatusResponse>> GetGenerationStatusAsync(
        string generationId,
        CancellationToken cancellationToken = default)
    {
        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "GET",
                Path = $"/generations/{generationId}"
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var credits = AsObject(payload?["credits"]);
        var error = AsObject(payload?["error"]);

        var response = new GenerationStatusResponse
        {
            GenerationId = payload?["generationId"]?.GetValue<string>() ?? generationId,
            Status = payload?["status"]?.GetValue<string>() ?? "unknown",
            GammaId = payload?["gammaId"]?.GetValue<string>(),
            GammaUrl = payload?["gammaUrl"]?.GetValue<string>(),
            ExportUrl = payload?["exportUrl"]?.GetValue<string>(),
            Credits = credits is null
                ? null
                : new CreditUsage
                {
                    Deducted = credits["deducted"]?.GetValue<int?>(),
                    Remaining = credits["remaining"]?.GetValue<int?>()
                },
            Error = error is null
                ? null
                : new GammaApiError
                {
                    Message = error["message"]?.GetValue<string>(),
                    Details = error
                },
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<GenerationStatusResponse>> WaitForCompletionAsync(
        string generationId,
        TimeSpan? pollingInterval = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        var interval = pollingInterval ?? _options.DefaultPollingInterval;
        var maxDuration = timeout ?? _options.DefaultWaitTimeout;
        var start = DateTimeOffset.UtcNow;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var status = await GetGenerationStatusAsync(generationId, cancellationToken);
            var state = status.Data.Status;
            if (string.Equals(state, "completed", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(state, "failed", StringComparison.OrdinalIgnoreCase))
            {
                return status;
            }

            if (DateTimeOffset.UtcNow - start >= maxDuration)
            {
                throw new TimeoutException($"Generation {generationId} did not reach a terminal state in {maxDuration}.");
            }

            await Task.Delay(interval, cancellationToken);
        }
    }

    public async Task<GammaApiResult<ThemeListResponse>> GetThemesAsync(
        ThemeQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "GET",
                Path = "/themes",
                Query = BuildThemeQuery(query)
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var items = ParseThemeItems(payload?["data"]);

        var response = new ThemeListResponse
        {
            Data = items,
            HasMore = payload?["hasMore"]?.GetValue<bool>() ?? false,
            NextCursor = payload?["nextCursor"]?.GetValue<string>(),
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<FolderListResponse>> GetFoldersAsync(
        FolderQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "GET",
                Path = "/folders",
                Query = BuildFolderQuery(query)
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var items = ParseFolderItems(payload?["data"]);

        var response = new FolderListResponse
        {
            Data = items,
            HasMore = payload?["hasMore"]?.GetValue<bool>() ?? false,
            NextCursor = payload?["nextCursor"]?.GetValue<string>(),
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<ArchiveGammaResponse>> ArchiveGammaAsync(
        string gammaId,
        CancellationToken cancellationToken = default)
    {
        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "POST",
                Path = $"/gammas/{gammaId}/archive"
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var response = new ArchiveGammaResponse
        {
            GammaId = payload?["gammaId"]?.GetValue<string>() ?? gammaId,
            Archived = payload?["archived"]?.GetValue<bool>() ?? false,
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    public async Task<GammaApiResult<DeleteGammaResponse>> DeleteGammaAsync(
        string gammaId,
        CancellationToken cancellationToken = default)
    {
        var raw = await _rawClient.SendAsync(
            new RawGammaRequest
            {
                Method = "DELETE",
                Path = $"/gammas/{gammaId}"
            },
            cancellationToken);

        EnsureSuccess(raw);

        var payload = AsObject(raw.Data.JsonBody);
        var response = new DeleteGammaResponse
        {
            Status = payload?["status"]?.GetValue<string>() ?? "unknown",
            GammaId = payload?["gammaId"]?.GetValue<string>() ?? gammaId,
            Message = payload?["message"]?.GetValue<string>(),
            RawBody = payload
        };

        return Wrap(raw, response);
    }

    private static JsonObject SerializeWithExtensionBag<T>(T request, IReadOnlyDictionary<string, JsonNode?>? additional)
    {
        var body = JsonSerializer.SerializeToNode(request, SerializerOptions) as JsonObject ?? new JsonObject();
        body.Remove("additionalParameters");

        if (additional is null)
        {
            return body;
        }

        foreach (var item in additional)
        {
            body[item.Key] = item.Value;
        }

        return body;
    }

    private static Dictionary<string, string?>? BuildThemeQuery(ThemeQuery? query)
    {
        if (query is null)
        {
            return null;
        }

        return new Dictionary<string, string?>
        {
            ["limit"] = query.Limit?.ToString(),
            ["after"] = query.After,
            ["query"] = query.Query,
            ["type"] = query.Type
        };
    }

    private static Dictionary<string, string?>? BuildFolderQuery(FolderQuery? query)
    {
        if (query is null)
        {
            return null;
        }

        return new Dictionary<string, string?>
        {
            ["limit"] = query.Limit?.ToString(),
            ["after"] = query.After,
            ["query"] = query.Query
        };
    }

    private static IReadOnlyList<ThemeItem> ParseThemeItems(JsonNode? node)
    {
        var list = new List<ThemeItem>();
        if (node is not JsonArray array)
        {
            return list;
        }

        foreach (var item in array)
        {
            var obj = AsObject(item);
            if (obj is null)
            {
                continue;
            }

            list.Add(new ThemeItem
            {
                Id = obj["id"]?.GetValue<string>() ?? string.Empty,
                Name = obj["name"]?.GetValue<string>() ?? string.Empty,
                Type = obj["type"]?.GetValue<string>(),
                ColorKeywords = ParseStringList(obj["colorKeywords"]),
                ToneKeywords = ParseStringList(obj["toneKeywords"])
            });
        }

        return list;
    }

    private static IReadOnlyList<FolderItem> ParseFolderItems(JsonNode? node)
    {
        var list = new List<FolderItem>();
        if (node is not JsonArray array)
        {
            return list;
        }

        foreach (var item in array)
        {
            var obj = AsObject(item);
            if (obj is null)
            {
                continue;
            }

            list.Add(new FolderItem
            {
                Id = obj["id"]?.GetValue<string>() ?? string.Empty,
                Name = obj["name"]?.GetValue<string>() ?? string.Empty
            });
        }

        return list;
    }

    private static IReadOnlyList<string>? ParseStringList(JsonNode? node)
    {
        if (node is not JsonArray array)
        {
            return null;
        }

        return array.Select(x => x?.GetValue<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>().ToList();
    }

    private static JsonObject? AsObject(JsonNode? node) => node as JsonObject;

    private static GammaApiResult<T> Wrap<T>(GammaApiResult<RawGammaResponse> raw, T data)
    {
        return new GammaApiResult<T>
        {
            StatusCode = raw.StatusCode,
            Data = data,
            RateLimit = raw.RateLimit
        };
    }

    private static void EnsureSuccess(GammaApiResult<RawGammaResponse> raw)
    {
        if (raw.StatusCode is >= 200 and <= 299)
        {
            return;
        }

        var message = ExtractErrorMessage(raw.Data.JsonBody) ?? $"Gamma API request failed with HTTP {raw.StatusCode}.";
        throw raw.StatusCode switch
        {
            400 => new GammaBadRequestException(message),
            401 => new GammaUnauthorizedException(message),
            402 => new GammaInsufficientCreditsException(message),
            403 => new GammaForbiddenException(message),
            404 => new GammaNotFoundException(message),
            429 => new GammaRateLimitException(message),
            >= 500 => new GammaServerException(message, raw.StatusCode),
            _ => new GammaApiException(message, raw.StatusCode)
        };
    }

    private static string? ExtractErrorMessage(JsonNode? body)
    {
        var obj = body as JsonObject;
        if (obj is null)
        {
            return null;
        }

        return obj["message"]?.GetValue<string>()
               ?? (obj["error"] as JsonObject)?["message"]?.GetValue<string>()
               ?? obj["error"]?.GetValue<string>();
    }
}
