using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Generations;

/// <summary>
/// Typed request for POST /v1.0/generations/from-template.
/// </summary>
public sealed class CreateFromTemplateRequest
{
    /// <summary>
    /// Template Gamma file identifier.
    /// </summary>
    public required string GammaId { get; init; }

    /// <summary>
    /// Instructions used to adapt the template.
    /// </summary>
    public required string Prompt { get; init; }

    /// <summary>
    /// Optional title for the generated Gamma.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Optional theme identifier applied to the generated Gamma.
    /// </summary>
    public string? ThemeId { get; init; }

    /// <summary>
    /// Optional target folders for the generated Gamma.
    /// </summary>
    public IReadOnlyList<string>? FolderIds { get; init; }

    /// <summary>
    /// Optional export format requested after generation completes.
    /// </summary>
    public string? ExportAs { get; init; }

    /// <summary>
    /// Optional image-generation overrides for template expansion.
    /// </summary>
    public TemplateImageOptions? ImageOptions { get; init; }

    /// <summary>
    /// Optional sharing configuration applied to the generated Gamma.
    /// </summary>
    public GenerationSharingOptions? SharingOptions { get; init; }

    /// <summary>
    /// Forward-compatible extension bag to pass new fields before typed model updates.
    /// </summary>
    public IReadOnlyDictionary<string, JsonNode?>? AdditionalParameters { get; init; }
}

public sealed class TemplateImageOptions
{
    /// <summary>
    /// Image generation model identifier.
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Optional free-form style description.
    /// </summary>
    public string? Style { get; init; }
}
