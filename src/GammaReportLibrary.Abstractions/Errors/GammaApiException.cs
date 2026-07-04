namespace GammaReportLibrary.Errors;

/// <summary>
/// Base exception for non-success HTTP responses returned by Gamma API.
/// </summary>
public class GammaApiException : Exception
{
    public GammaApiException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

public sealed class GammaBadRequestException : GammaApiException
{
    public GammaBadRequestException(string message) : base(message, 400) { }
}

public sealed class GammaUnauthorizedException : GammaApiException
{
    public GammaUnauthorizedException(string message) : base(message, 401) { }
}

public sealed class GammaInsufficientCreditsException : GammaApiException
{
    public GammaInsufficientCreditsException(string message) : base(message, 402) { }
}

public sealed class GammaForbiddenException : GammaApiException
{
    public GammaForbiddenException(string message) : base(message, 403) { }
}

public sealed class GammaNotFoundException : GammaApiException
{
    public GammaNotFoundException(string message) : base(message, 404) { }
}

public sealed class GammaRateLimitException : GammaApiException
{
    public GammaRateLimitException(string message) : base(message, 429) { }
}

public sealed class GammaServerException : GammaApiException
{
    public GammaServerException(string message, int statusCode) : base(message, statusCode) { }
}
