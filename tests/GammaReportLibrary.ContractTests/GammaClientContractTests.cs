using System.Text.Json.Nodes;
using GammaReportLibrary.Errors;
using Microsoft.Extensions.Options;
using GammaReportLibrary.Models.Raw;
using GammaReportLibrary.Application;
using GammaReportLibrary.Abstractions;
using GammaReportLibrary.Models.Client;
using GammaReportLibrary.Models.Common;
using GammaReportLibrary.Models.Generations;

namespace GammaReportLibrary.ContractTests;

public class GammaClientContractTests
{
    [Fact]
    public async Task CreateGenerationAsync_MapsPayloadAndRateLimit()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "generationId": "abc123",
                  "status": "pending",
                  "warnings": "cardOptions.dimensions was normalized"
                }
                """)
            },
            RateLimit = new RateLimitMetadata { RemainingBurst = 999 }
        };

        var sut = BuildSut(rawResult);

        var result = await sut.CreateGenerationAsync(new CreateGenerationRequest
        {
            InputText = "Q3 strategy"
        });

        Assert.Equal("abc123", result.Data.GenerationId);
        Assert.Equal("pending", result.Data.Status);
        Assert.Equal(999, result.RateLimit?.RemainingBurst);
        Assert.Contains("normalized", result.Data.Warnings);
    }

    [Fact]
    public async Task GetGenerationStatusAsync_MapsCompletedFields()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "generationId": "abc123",
                  "status": "completed",
                  "gammaId": "g_l0mf2jvf1fpmi1v",
                  "gammaUrl": "https://gamma.app/docs/abc123",
                  "exportUrl": "https://gamma.app/export/abc123.pdf",
                  "credits": {
                    "deducted": 15,
                    "remaining": 485
                  }
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);

        var result = await sut.GetGenerationStatusAsync("abc123");

        Assert.Equal("completed", result.Data.Status);
        Assert.Equal("g_l0mf2jvf1fpmi1v", result.Data.GammaId);
        Assert.Equal(15, result.Data.Credits?.Deducted);
        Assert.Equal(485, result.Data.Credits?.Remaining);
    }

    [Fact]
    public async Task GetThemesAsync_MapsPaginatedResponse()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "data": [
                    {
                      "id": "theme_1",
                      "name": "Prism",
                      "type": "custom",
                      "colorKeywords": ["blue"],
                      "toneKeywords": ["professional"]
                    }
                  ],
                  "hasMore": true,
                  "nextCursor": "cursor_1"
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);
        var result = await sut.GetThemesAsync();

        Assert.Single(result.Data.Data);
        Assert.True(result.Data.HasMore);
        Assert.Equal("cursor_1", result.Data.NextCursor);
        Assert.Equal("theme_1", result.Data.Data[0].Id);
    }

    [Fact]
    public async Task GetFoldersAsync_MapsPaginatedResponse()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "data": [
                    { "id": "folder_1", "name": "Sales" }
                  ],
                  "hasMore": false,
                  "nextCursor": null
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);
        var result = await sut.GetFoldersAsync();

        Assert.Single(result.Data.Data);
        Assert.False(result.Data.HasMore);
        Assert.Equal("folder_1", result.Data.Data[0].Id);
    }

    [Fact]
    public async Task ArchiveGammaAsync_MapsArchiveResponse()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "gammaId": "g_123",
                  "archived": true
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);
        var result = await sut.ArchiveGammaAsync("g_123");

        Assert.Equal("g_123", result.Data.GammaId);
        Assert.True(result.Data.Archived);
    }

    [Fact]
    public async Task DeleteGammaAsync_MapsDeleteResponse()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse("""
                {
                  "status": "deleted",
                  "gammaId": "g_123",
                  "message": "Deleted"
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);
        var result = await sut.DeleteGammaAsync("g_123");

        Assert.Equal("deleted", result.Data.Status);
        Assert.Equal("g_123", result.Data.GammaId);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForUnauthorized()
    {
        var rawResult = new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 401,
            Data = new RawGammaResponse
            {
                StatusCode = 401,
                JsonBody = JsonNode.Parse("""
                {
                  "message": "Invalid API key."
                }
                """)
            }
        };

        var sut = BuildSut(rawResult);

        var ex = await Assert.ThrowsAsync<GammaUnauthorizedException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Contains("Invalid API key", ex.Message);
        Assert.Equal(401, ex.StatusCode);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForInsufficientCredits()
    {
        var sut = BuildSut(ErrorResponse(402, "Insufficient credits remaining"));

        var ex = await Assert.ThrowsAsync<GammaInsufficientCreditsException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Equal(402, ex.StatusCode);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForForbidden()
    {
        var sut = BuildSut(ErrorResponse(403, "Forbidden"));

        var ex = await Assert.ThrowsAsync<GammaForbiddenException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Equal(403, ex.StatusCode);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForNotFound()
    {
        var sut = BuildSut(ErrorResponse(404, "Not found"));

        var ex = await Assert.ThrowsAsync<GammaNotFoundException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Equal(404, ex.StatusCode);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForRateLimit()
    {
        var sut = BuildSut(ErrorResponse(429, "Too many requests"));

        var ex = await Assert.ThrowsAsync<GammaRateLimitException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Equal(429, ex.StatusCode);
    }

    [Fact]
    public async Task CreateGenerationAsync_ThrowsMappedExceptionForServerError()
    {
        var sut = BuildSut(ErrorResponse(500, "Internal server error"));

        var ex = await Assert.ThrowsAsync<GammaServerException>(() =>
            sut.CreateGenerationAsync(new CreateGenerationRequest { InputText = "x" }));

        Assert.Equal(500, ex.StatusCode);
    }

    [Fact]
    public async Task WaitForCompletionAsync_ReturnsCompletedAfterPending()
    {
        var sut = BuildSut(
            SuccessStatusResponse("job_1", "pending"),
            SuccessStatusResponse("job_1", "completed"));

        var result = await sut.WaitForCompletionAsync(
            "job_1",
            pollingInterval: TimeSpan.FromMilliseconds(1),
            timeout: TimeSpan.FromMilliseconds(100));

        Assert.Equal("completed", result.Data.Status);
        Assert.Equal("job_1", result.Data.GenerationId);
    }

    [Fact]
    public async Task WaitForCompletionAsync_ReturnsFailedWhenTerminalStateIsFailed()
    {
        var sut = BuildSut(
            SuccessStatusResponse("job_2", "pending"),
            SuccessStatusResponse("job_2", "failed", includeError: true));

        var result = await sut.WaitForCompletionAsync(
            "job_2",
            pollingInterval: TimeSpan.FromMilliseconds(1),
            timeout: TimeSpan.FromMilliseconds(100));

        Assert.Equal("failed", result.Data.Status);
        Assert.NotNull(result.Data.Error);
    }

    [Fact]
    public async Task WaitForCompletionAsync_ThrowsTimeoutException_WhenNotTerminal()
    {
        var sut = BuildSut(
            SuccessStatusResponse("job_3", "pending"),
            SuccessStatusResponse("job_3", "pending"),
            SuccessStatusResponse("job_3", "pending"));

        await Assert.ThrowsAsync<TimeoutException>(() => sut.WaitForCompletionAsync(
            "job_3",
            pollingInterval: TimeSpan.FromMilliseconds(1),
            timeout: TimeSpan.FromMilliseconds(2)));
    }

    [Fact]
    public async Task WaitForCompletionAsync_ThrowsOperationCanceledException_WhenCanceled()
    {
        var sut = BuildSut(SuccessStatusResponse("job_4", "pending"));
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.WaitForCompletionAsync(
            "job_4",
            cancellationToken: cts.Token));
    }

    private static IGammaClient BuildSut(GammaApiResult<RawGammaResponse> rawResult)
    {
        IGammaRawClient rawClient = new FakeRawClient(rawResult);
        var options = Options.Create(new GammaClientOptions
        {
            ApiKey = "test-key",
            DefaultPollingInterval = TimeSpan.FromMilliseconds(1),
            DefaultWaitTimeout = TimeSpan.FromMilliseconds(50)
        });

        return new GammaClientApplicationService(rawClient, options);
    }

    private static IGammaClient BuildSut(params GammaApiResult<RawGammaResponse>[] rawResults)
    {
        IGammaRawClient rawClient = new FakeRawClient(rawResults);
        var options = Options.Create(new GammaClientOptions
        {
            ApiKey = "test-key",
            DefaultPollingInterval = TimeSpan.FromMilliseconds(1),
            DefaultWaitTimeout = TimeSpan.FromMilliseconds(50)
        });

        return new GammaClientApplicationService(rawClient, options);
    }

    private static GammaApiResult<RawGammaResponse> ErrorResponse(int statusCode, string message)
    {
        return new GammaApiResult<RawGammaResponse>
        {
            StatusCode = statusCode,
            Data = new RawGammaResponse
            {
                StatusCode = statusCode,
                JsonBody = JsonNode.Parse($$"""
                {
                  "message": "{{message}}"
                }
                """
)
            }
        };
    }

    private static GammaApiResult<RawGammaResponse> SuccessStatusResponse(string generationId, string status, bool includeError = false)
    {
        var errorJson = includeError ? ",\n  \"error\": { \"message\": \"Generation failed\" }" : string.Empty;
        return new GammaApiResult<RawGammaResponse>
        {
            StatusCode = 200,
            Data = new RawGammaResponse
            {
                StatusCode = 200,
                JsonBody = JsonNode.Parse($$"""
                {
                  "generationId": "{{generationId}}",
                  "status": "{{status}}"{{errorJson}}
                }
                """)
            }
        };
    }

    private sealed class FakeRawClient : IGammaRawClient
    {
        private readonly Queue<GammaApiResult<RawGammaResponse>> _results;

        public FakeRawClient(params GammaApiResult<RawGammaResponse>[] results)
        {
            _results = new Queue<GammaApiResult<RawGammaResponse>>(results);
        }

        public Task<GammaApiResult<RawGammaResponse>> SendAsync(RawGammaRequest request, CancellationToken cancellationToken = default)
        {
            if (_results.Count == 0)
            {
                throw new InvalidOperationException("No fake response available.");
            }

            return Task.FromResult(_results.Dequeue());
        }
    }
}
