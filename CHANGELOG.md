# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2026-07-04

### Added

- Public NuGet package surface through the `GammaReportLibrary` facade project.
- Dedicated `GammaReportLibrary.Abstractions` project for public contracts, DTOs, options, raw payloads, and error types.
- Typed client `IGammaClient` for the main Gamma API operations.
- Raw client `IGammaRawClient` for low-level request and response access.
- Support for creating generations and template-based generations.
- Support for checking generation status and waiting for completion.
- Support for listing themes and folders.
- Support for archiving and deleting Gamma items.
- HTTP error mapping for `400`, `401`, `402`, `403`, `404`, `429`, and `5xx` responses.
- Retry and backoff handling for transient `429` and `5xx` failures, including `Retry-After` support.
- Exposure of rate-limit metadata for consumers that need quota visibility.
- XML documentation on the public surface for IntelliSense.
- Packaging metadata and README configuration ready for nuget.org publication.
