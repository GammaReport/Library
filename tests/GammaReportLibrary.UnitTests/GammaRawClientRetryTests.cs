using System.Net;
using System.Text;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using GammaReportLibrary.Models.Raw;
using GammaReportLibrary.Models.Client;
using GammaReportLibrary.Infrastructure;

namespace GammaReportLibrary.UnitTests;

public class GammaRawClientRetryTests
{
    [Fact]
    public async Task SendAsync_RetriesOn429_AndEventuallyReturnsSuccess()
    {
        var handler = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent("{\"message\":\"rate limited\"}", Encoding.UTF8, "application/json")
            },
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"generationId\":\"ok-1\"}", Encoding.UTF8, "application/json")
            });

        var client = BuildSut(handler, maxRetryAttempts: 2);
        var result = await client.SendAsync(new RawGammaRequest { Method = "GET", Path = "/generations/ok-1" });

        Assert.Equal(200, result.StatusCode);
        Assert.Equal(2, handler.SendCount);
        Assert.Equal("ok-1", (result.Data.JsonBody as JsonObject)?["generationId"]?.GetValue<string>());
    }

    [Fact]
    public async Task SendAsync_DoesNotRetryOn400()
    {
        var handler = new SequenceHandler(
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"message\":\"invalid\"}", Encoding.UTF8, "application/json")
            },
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"generationId\":\"unexpected\"}", Encoding.UTF8, "application/json")
            });

        var client = BuildSut(handler, maxRetryAttempts: 2);
        var result = await client.SendAsync(new RawGammaRequest { Method = "GET", Path = "/generations/bad" });

        Assert.Equal(400, result.StatusCode);
        Assert.Equal(1, handler.SendCount);
    }

    [Fact]
    public async Task SendAsync_RespectsRetryAfterHeader_WhenProvided()
    {
        var tooMany = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("{\"message\":\"rate limited\"}", Encoding.UTF8, "application/json")
        };
        tooMany.Headers.Add("Retry-After", "1");

        var ok = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"generationId\":\"ok-2\"}", Encoding.UTF8, "application/json")
        };

        var handler = new SequenceHandler(tooMany, ok);
        var client = BuildSut(handler, maxRetryAttempts: 2);

        var sw = Stopwatch.StartNew();
        var result = await client.SendAsync(new RawGammaRequest { Method = "GET", Path = "/generations/ok-2" });
        sw.Stop();

        Assert.Equal(200, result.StatusCode);
        Assert.Equal(2, handler.SendCount);
        Assert.True(sw.Elapsed >= TimeSpan.FromMilliseconds(900));
    }

    private static GammaRawClient BuildSut(HttpMessageHandler handler, int maxRetryAttempts)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://public-api.gamma.app/v1.0")
        };

        var options = Options.Create(new GammaClientOptions
        {
            ApiKey = "test-key",
            MaxRetryAttempts = maxRetryAttempts,
            RetryBaseDelay = TimeSpan.FromMilliseconds(1),
            RetryMaxDelay = TimeSpan.FromMilliseconds(5)
        });

        return new GammaRawClient(httpClient, options);
    }

    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        public SequenceHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        public int SendCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SendCount++;
            if (_responses.Count == 0)
            {
                throw new InvalidOperationException("No queued response available.");
            }

            return Task.FromResult(_responses.Dequeue());
        }
    }
}
