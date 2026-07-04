using GammaReportLibrary.Models.Common;
using GammaReportLibrary.Models.Gammas;
using GammaReportLibrary.Models.Workspace;
using GammaReportLibrary.Models.Generations;

namespace GammaReportLibrary.Abstractions;

/// <summary>
/// Provides the typed high-level API surface for Gamma Public API operations.
/// </summary>
public interface IGammaClient
{
    /// <summary>
    /// Starts an asynchronous generation from text content.
    /// </summary>
    /// <param name="request">Typed request containing all supported generation parameters.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The typed Gamma API response with transport metadata.</returns>
    Task<GammaApiResult<StartGenerationResponse>> CreateGenerationAsync(
        CreateGenerationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts an asynchronous generation from an existing template.
    /// </summary>
    /// <param name="request">Typed template-generation request.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The typed Gamma API response with transport metadata.</returns>
    Task<GammaApiResult<StartGenerationResponse>> CreateFromTemplateAsync(
        CreateFromTemplateRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the current status of a generation.
    /// </summary>
    /// <param name="generationId">Identifier returned by a create-generation call.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The current generation status payload.</returns>
    Task<GammaApiResult<GenerationStatusResponse>> GetGenerationStatusAsync(
        string generationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Polls generation status until it reaches a terminal state.
    /// </summary>
    /// <param name="generationId">Identifier returned by a create-generation call.</param>
    /// <param name="pollingInterval">Optional polling interval override.</param>
    /// <param name="timeout">Optional timeout override for the polling loop.</param>
    /// <param name="cancellationToken">Cancels the polling operation.</param>
    /// <returns>The terminal generation status payload.</returns>
    Task<GammaApiResult<GenerationStatusResponse>> WaitForCompletionAsync(
        string generationId,
        TimeSpan? pollingInterval = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists workspace themes.
    /// </summary>
    /// <param name="query">Optional pagination and filtering parameters.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The paginated themes response.</returns>
    Task<GammaApiResult<ThemeListResponse>> GetThemesAsync(
        ThemeQuery? query = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists workspace folders.
    /// </summary>
    /// <param name="query">Optional pagination and filtering parameters.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The paginated folders response.</returns>
    Task<GammaApiResult<FolderListResponse>> GetFoldersAsync(
        FolderQuery? query = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Archives a Gamma document.
    /// </summary>
    /// <param name="gammaId">Gamma file identifier returned by the API.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The archive operation response.</returns>
    Task<GammaApiResult<ArchiveGammaResponse>> ArchiveGammaAsync(
        string gammaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a Gamma document.
    /// </summary>
    /// <param name="gammaId">Gamma file identifier returned by the API.</param>
    /// <param name="cancellationToken">Cancels the outbound request.</param>
    /// <returns>The delete operation response.</returns>
    Task<GammaApiResult<DeleteGammaResponse>> DeleteGammaAsync(
        string gammaId,
        CancellationToken cancellationToken = default);
}
