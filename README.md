# GammaReportLibrary

Libreria .NET 10 para consumir Gamma Public API con enfoque de clean architecture, superficie tipada (`IGammaClient`) y acceso de bajo nivel (`IGammaRawClient`) para paridad completa.

## Que incluye hoy
- Cliente tipado para endpoints principales de Gamma API.
- Cliente raw para request/response completo.
- Mapeo de errores HTTP a excepciones de dominio.
- Retry/backoff para `429` y `5xx` configurable.
- Exposicion de metadatos de rate limit.

## Instalacion en la solucion
Actualmente el repositorio esta organizado por proyectos en `src/`.

## Registro por DI
La extension de registro vive en infraestructura.

```csharp
using GammaReportLibrary.Infrastructure.DependencyInjection;

var services = new ServiceCollection();

services.AddGammaReportLibrary(options =>
{
    options.ApiKey = "sk-gamma-xxxxx";
    options.BaseUrl = "https://public-api.gamma.app/v1.0";
    options.MaxRetryAttempts = 2;
    options.RetryBaseDelay = TimeSpan.FromSeconds(1);
    options.RetryMaxDelay = TimeSpan.FromSeconds(30);
    options.DefaultPollingInterval = TimeSpan.FromSeconds(5);
    options.DefaultWaitTimeout = TimeSpan.FromMinutes(5);
});
```

## Uso rapido

```csharp
using GammaReportLibrary.Abstractions;
using GammaReportLibrary.Models.Generations;

var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IGammaClient>();

var started = await client.CreateGenerationAsync(new CreateGenerationRequest
{
    InputText = "Q3 product launch strategy",
    TextMode = "generate",
    Format = "presentation",
    NumCards = 10,
    ExportAs = "pdf"
});

var done = await client.WaitForCompletionAsync(started.Data.GenerationId);

Console.WriteLine($"Status: {done.Data.Status}");
Console.WriteLine($"Gamma URL: {done.Data.GammaUrl}");
Console.WriteLine($"Export URL: {done.Data.ExportUrl}");
```

## Sample ejecutable
Existe un ejemplo minimo listo para ejecutar en:
- `samples/GammaReportLibrary.Sample`

Variables necesarias:
- `GAMMA_API_KEY`

Ejecucion:

```bash
export GAMMA_API_KEY="sk-gamma-xxxxx"
dotnet run --project samples/GammaReportLibrary.Sample/GammaReportLibrary.Sample.csproj
```

## Errores mapeados
El cliente tipado lanza excepciones de dominio en no-exito HTTP:
- `GammaBadRequestException` (`400`)
- `GammaUnauthorizedException` (`401`)
- `GammaInsufficientCreditsException` (`402`)
- `GammaForbiddenException` (`403`)
- `GammaNotFoundException` (`404`)
- `GammaRateLimitException` (`429`)
- `GammaServerException` (`5xx`)

## Documentacion en IDE
Los tipos publicos estan preparados para documentacion de IntelliSense via comentarios XML. La guia de consumo de alto nivel se mantiene en este `README`.

## Estado
Base de implementacion activa. Se continua ampliando cobertura de paridad endpoint-parametro-campo en la planificacion interna del proyecto.
