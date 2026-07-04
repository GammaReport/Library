using System.Text.Json.Nodes;

namespace GammaReportLibrary.Models.Generations;

/// <summary>
/// Typed request for POST /v1.0/generations.
/// </summary>
public sealed class CreateGenerationRequest
{
    /// <summary>
    /// Input content used by Gamma to create the artifact.
    /// </summary>
    public required string InputText { get; init; }

    /// <summary>
    /// Optional explicit title for the generated Gamma.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Controls how Gamma interprets the input text.
    /// </summary>
    public string? TextMode { get; init; }

    /// <summary>
    /// Target output format such as presentation, document, social or webpage.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Desired number of cards or sections when card splitting is automatic.
    /// </summary>
    public int? NumCards { get; init; }

    /// <summary>
    /// Controls how Gamma splits content into cards.
    /// </summary>
    public string? CardSplit { get; init; }

    /// <summary>
    /// Theme identifier returned by the themes endpoint.
    /// </summary>
    public string? ThemeId { get; init; }

    /// <summary>
    /// Optional target folders for the generated Gamma.
    /// </summary>
    public IReadOnlyList<string>? FolderIds { get; init; }

    /// <summary>
    /// Additional natural-language instructions for layout or content behavior.
    /// </summary>
    public string? AdditionalInstructions { get; init; }

    /// <summary>
    /// Optional export format requested after generation completes.
    /// </summary>
    public string? ExportAs { get; init; }

    /// <summary>
    /// Optional text-generation settings.
    /// </summary>
    public GenerationTextOptions? TextOptions { get; init; }

    /// <summary>
    /// Optional image-generation or image-source settings.
    /// </summary>
    public GenerationImageOptions? ImageOptions { get; init; }

    /// <summary>
    /// Optional card layout and header/footer settings.
    /// </summary>
    public GenerationCardOptions? CardOptions { get; init; }

    /// <summary>
    /// Optional sharing configuration applied to the generated Gamma.
    /// </summary>
    public GenerationSharingOptions? SharingOptions { get; init; }

    /// <summary>
    /// Forward-compatible extension bag to pass new fields before typed model updates.
    /// </summary>
    public IReadOnlyDictionary<string, JsonNode?>? AdditionalParameters { get; init; }
}

public sealed class GenerationTextOptions
{
    /// <summary>
    /// Controls the amount of text generated per card.
    /// </summary>
    public string? Amount { get; init; }

    /// <summary>
    /// Desired writing tone.
    /// </summary>
    public string? Tone { get; init; }

    /// <summary>
    /// Intended audience for the content.
    /// </summary>
    public string? Audience { get; init; }

    /// <summary>
    /// Output language code.
    /// </summary>
    public string? Language { get; init; }
}

/// <summary>
/// Image-related options for standard generation.
/// </summary>
public sealed class GenerationImageOptions
{
    /// <summary>
    /// Source from which Gamma should obtain images.
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Image generation model identifier when image generation is enabled.
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Optional image style preset.
    /// </summary>
    public string? StylePreset { get; init; }

    /// <summary>
    /// Optional free-form style description.
    /// </summary>
    public string? Style { get; init; }
}

/// <summary>
/// Layout options for generated cards.
/// </summary>
public sealed class GenerationCardOptions
{
    /// <summary>
    /// Aspect ratio or page size for the generated output.
    /// </summary>
    public string? Dimensions { get; init; }

    /// <summary>
    /// Optional header and footer configuration.
    /// </summary>
    public GenerationHeaderFooterOptions? HeaderFooter { get; init; }
}

/// <summary>
/// Header and footer slot configuration across generated cards.
/// </summary>
public sealed class GenerationHeaderFooterOptions
{
    /// <summary>
    /// Top-left slot content.
    /// </summary>
    public HeaderFooterSlot? TopLeft { get; init; }

    /// <summary>
    /// Top-center slot content.
    /// </summary>
    public HeaderFooterSlot? TopCenter { get; init; }

    /// <summary>
    /// Top-right slot content.
    /// </summary>
    public HeaderFooterSlot? TopRight { get; init; }

    /// <summary>
    /// Bottom-left slot content.
    /// </summary>
    public HeaderFooterSlot? BottomLeft { get; init; }

    /// <summary>
    /// Bottom-center slot content.
    /// </summary>
    public HeaderFooterSlot? BottomCenter { get; init; }

    /// <summary>
    /// Bottom-right slot content.
    /// </summary>
    public HeaderFooterSlot? BottomRight { get; init; }

    /// <summary>
    /// Hides configured header/footer content on the first card.
    /// </summary>
    public bool? HideFromFirstCard { get; init; }

    /// <summary>
    /// Hides configured header/footer content on the last card.
    /// </summary>
    public bool? HideFromLastCard { get; init; }
}

/// <summary>
/// Represents one configurable header or footer slot.
/// </summary>
public sealed class HeaderFooterSlot
{
    /// <summary>
    /// Slot content type such as text, image or cardNumber.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Image source when the slot uses an image.
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Custom image URL when source is custom.
    /// </summary>
    public string? Src { get; init; }

    /// <summary>
    /// Text value when the slot uses text.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Image size hint for logo/image slots.
    /// </summary>
    public string? Size { get; init; }
}

/// <summary>
/// Sharing-related options for generated Gamma content.
/// </summary>
public sealed class GenerationSharingOptions
{
    /// <summary>
    /// Access level for workspace members.
    /// </summary>
    public string? WorkspaceAccess { get; init; }

    /// <summary>
    /// Access level for users outside the workspace.
    /// </summary>
    public string? ExternalAccess { get; init; }

    /// <summary>
    /// Email-based sharing options.
    /// </summary>
    public GenerationEmailOptions? EmailOptions { get; init; }
}

/// <summary>
/// Email sharing options.
/// </summary>
public sealed class GenerationEmailOptions
{
    /// <summary>
    /// Email recipients that should receive access to the Gamma.
    /// </summary>
    public IReadOnlyList<string>? Recipients { get; init; }

    /// <summary>
    /// Access level granted to the configured recipients.
    /// </summary>
    public string? Access { get; init; }
}
