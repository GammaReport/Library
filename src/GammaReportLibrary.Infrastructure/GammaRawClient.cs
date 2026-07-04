using System.Text;
using System.Text.Json.Nodes;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using GammaReportLibrary.Models.Raw;
using GammaReportLibrary.Abstractions;
using GammaReportLibrary.Models.Client;
using GammaReportLibrary.Models.Common;

namespace GammaReportLibrary.Infrastructure;

public sealed class GammaRawClient : IGammaRawClient
{
    private readonly HttpClient _httpClient;
    private readonly GammaClientOptions _options;

    public GammaRawClient(HttpClient httpClient, IOptions<GammaClientOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<GammaApiResult<RawGammaResponse>> SendAsync(
        RawGammaRequest request,
        CancellationToken cancellationToken = default)
    {
        var maxRetries = Math.Max(0, _options.MaxRetryAttempts);
        for (var attempt = 0; ; attempt++)
        {
            using var message = BuildHttpRequestMessage(request);
            using var response = await _httpClient.SendAsync(message, cancellationToken);
            var rawTextBody = response.Content is null
                ? null
                : await response.Content.ReadAsStringAsync(cancellationToken);

            JsonNode? jsonBody = null;
            if (!string.IsNullOrWhiteSpace(rawTextBody))
            {
                try
                {
                    jsonBody = JsonNode.Parse(rawTextBody);
                }
                catch
                {
                    jsonBody = null;
                }
            }

            var headers = GetHeaders(response);
            var statusCode = (int)response.StatusCode;

            if (ShouldRetry(statusCode) && attempt < maxRetries)
            {
                var delay = GetRetryDelay(headers, attempt);
                await Task.Delay(delay, cancellationToken);
                continue;
            }

            var result = new RawGammaResponse
            {
                StatusCode = statusCode,
                Headers = headers,
                JsonBody = jsonBody,
                RawTextBody = rawTextBody
            };

            return new GammaApiResult<RawGammaResponse>
            {
                StatusCode = statusCode,
                Data = result,
                RateLimit = BuildRateLimitMetadata(headers)
            };
        }
    }

    private HttpRequestMessage BuildHttpRequestMessage(RawGammaRequest request)
    {
        var message = new HttpRequestMessage(new HttpMethod(request.Method), BuildRelativePath(request));

        if (!string.IsNullOrWhiteSpace(_options.ApiKey) &&
            (request.Headers is null || !request.Headers.ContainsKey("X-API-KEY")))
        {
            message.Headers.TryAddWithoutValidation("X-API-KEY", _options.ApiKey);
        }

        if (request.Headers is not null)
        {
            foreach (var item in request.Headers)
            {
                if (item.Value is null)
                {
                    continue;
                }

                message.Headers.TryAddWithoutValidation(item.Key, item.Value);
            }
        }

        if (request.JsonBody is not null)
        {
            var json = request.JsonBody.ToJsonString();
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        return message;
    }

    private static bool ShouldRetry(int statusCode)
    {
        return statusCode == 429 || statusCode >= 500;
    }

    private TimeSpan GetRetryDelay(IReadOnlyDictionary<string, IReadOnlyList<string>> headers, int attempt)
    {
        if (headers.TryGetValue("retry-after", out var retryAfterValues) && retryAfterValues.Count > 0)
        {
            if (int.TryParse(retryAfterValues[0], out var seconds) && seconds > 0)
            {
                return TimeSpan.FromSeconds(seconds);
            }
        }

        var exponent = Math.Pow(2, attempt);
        var delayMs = _options.RetryBaseDelay.TotalMilliseconds * exponent;
        delayMs = Math.Min(delayMs, _options.RetryMaxDelay.TotalMilliseconds);
        if (delayMs < 1)
        {
            delayMs = 1;
        }

        return TimeSpan.FromMilliseconds(delayMs);
    }

    private static string BuildRelativePath(RawGammaRequest request)
    {
        var normalizedPath = request.Path.TrimStart('/');

        if (request.Query is null || request.Query.Count == 0)
        {
            return normalizedPath;
        }

        var pairs = request.Query
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value!)}");

        var queryString = string.Join("&", pairs);
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return normalizedPath;
        }

        var separator = normalizedPath.Contains('?') ? "&" : "?";
        return $"{normalizedPath}{separator}{queryString}";
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<string>> GetHeaders(HttpResponseMessage response)
    {
        var headers = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
        {
            headers[header.Key] = header.Value.ToList();
        }

        if (response.Content is not null)
        {
            foreach (var header in response.Content.Headers)
            {
                headers[header.Key] = header.Value.ToList();
            }
        }

        return headers;
    }

    private static RateLimitMetadata BuildRateLimitMetadata(IReadOnlyDictionary<string, IReadOnlyList<string>> headers)
    {
        return new RateLimitMetadata
        {
            LimitBurst = GetIntHeader(headers, "x-ratelimit-limit-burst"),
            RemainingBurst = GetIntHeader(headers, "x-ratelimit-remaining-burst"),
            LimitHourly = GetIntHeader(headers, "x-ratelimit-limit"),
            RemainingHourly = GetIntHeader(headers, "x-ratelimit-remaining"),
            LimitDaily = GetIntHeader(headers, "x-ratelimit-limit-daily"),
            RemainingDaily = GetIntHeader(headers, "x-ratelimit-remaining-daily")
        };
    }

    private static int? GetIntHeader(IReadOnlyDictionary<string, IReadOnlyList<string>> headers, string key)
    {
        if (!headers.TryGetValue(key, out var values) || values.Count == 0)
        {
            return null;
        }

        return int.TryParse(values[0], out var value) ? value : null;
    }
}
