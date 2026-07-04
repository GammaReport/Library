# Backlog evolutivo - GammaReportLibrary

## 1. Como mantener este backlog
- Estado permitido por tarea: `todo`, `in-progress`, `blocked`, `done`.
- Toda tarea debe referenciar al menos un requerimiento (`FR-*` o `NFR-*`).
- Politica de compatibilidad vigente: hasta la primera publicacion oficial de NuGet, el proyecto se trata como greenfield y no se exige compatibilidad hacia atras.
- Para nuevos cambios futuros:
  1. agregar requerimiento en `02-requerimientos.md`
  2. crear o actualizar tarea aqui
  3. registrar decision arquitectonica si aplica

## 2. Roadmap por epicas

## EPIC-01 Transporte y autenticacion
Objetivo: establecer base de comunicacion HTTP segura y extensible.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-001 | Crear contrato `IGammaTransport` | FR-001, NFR-002 | todo | Abstraccion de transporte |
| T-002 | Implementar cliente HTTP base con `HttpClientFactory` | FR-001, NFR-006 | todo | DI-friendly |
| T-003 | Inyectar `X-API-KEY` y `Content-Type` en requests | FR-001 | todo | Validar no nulo |
| T-004 | Normalizar manejo de errores HTTP a dominio | FR-010 | todo | Incluye 4xx/5xx |

## EPIC-02 Generaciones y polling
Objetivo: soportar flujo asincrono principal de Gamma API.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-005 | Modelar request/response para create generation | FR-002, FR-014, FR-017 | todo | Cobertura total de parametros/campos |
| T-006 | Implementar `CreateGenerationAsync` | FR-002 | todo | Endpoint `/generations` |
| T-007 | Modelar request/response from-template | FR-003, FR-015, FR-017 | todo | Incluye warnings y campos opcionales |
| T-008 | Implementar `CreateFromTemplateAsync` | FR-003 | todo | Endpoint `/generations/from-template` |
| T-009 | Implementar `GetGenerationStatusAsync` | FR-004, FR-017 | todo | Endpoint `/generations/{id}` |
| T-010 | Implementar `WaitForCompletionAsync` | FR-005, NFR-004 | todo | Polling configurable + cancelacion |

## EPIC-03 Recursos de workspace
Objetivo: exponer catalogos de themes y folders.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-011 | Modelar paginacion comun (`hasMore`, `nextCursor`) | FR-006, FR-007 | todo | Reutilizable |
| T-012 | Implementar `GetThemesAsync` | FR-006, FR-016 | todo | Query params limit/after/type/query |
| T-013 | Implementar `GetFoldersAsync` | FR-007, FR-016 | todo | Query params limit/after/query |

## EPIC-04 Gestion de gammas
Objetivo: administrar ciclo de vida de gamma generado.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-014 | Implementar `ArchiveGammaAsync` | FR-008 | todo | Idempotente |
| T-015 | Implementar `DeleteGammaAsync` | FR-009 | todo | Requiere rol admin |

## EPIC-05 Resiliencia y observabilidad
Objetivo: mejorar robustez operativa y control de consumo.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-016 | Politica retry/backoff para `429` y `5xx` | FR-010, NFR-004 | todo | Configurable |
| T-017 | Exponer metadatos de rate limit | FR-011 | todo | Headers en resultado |
| T-018 | Logging estructurado sin secretos | NFR-005, NFR-007 | todo | Sanitizar payloads |

## EPIC-06 Calidad, documentacion y publicacion
Objetivo: dejar la libreria reusable y lista para versionar.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-019 | Definir pruebas unitarias capa Domain/Application | NFR-003 | todo | Cobertura >= 80% |
| T-020 | Definir contract tests de serializacion | NFR-003 | todo | Casos reales de docs |
| T-021 | Crear documentacion de consumo y ejemplos | FR-012, NFR-006 | todo | IntelliSense XML + README/Markdown + snippets |
| T-022 | Configurar empaquetado NuGet | NFR-006, NFR-008 | todo | Metadatos completos |
| T-023 | Establecer versionado semantico y changelog | NFR-008 | todo | Plantilla de release |

## EPIC-07 Paridad completa API surface
Objetivo: garantizar que la libreria exponga toda la capacidad de Gamma API sin bypass.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-024 | Crear matriz de paridad endpoint-parametro-campo | FR-013, NFR-009 | todo | Artefacto vivo en docs |
| T-025 | Auditar `POST /generations` parametro por parametro | FR-014, FR-018 | todo | Incluye objetos anidados |
| T-026 | Auditar `POST /generations/from-template` parametro por parametro | FR-015, FR-018 | todo | Incluye image/sharing options |
| T-027 | Auditar `GET /themes` y `GET /folders` query params completos | FR-016 | todo | Paginacion y filtros |
| T-028 | Auditar campos de respuesta en todos los endpoints | FR-017 | todo | warnings, credits, error, urls |
| T-029 | Definir y publicar `IGammaRawClient` | FR-019, NFR-006 | todo | Camino de control absoluto |
| T-030 | Evitar defaults implicitos que alteren request | FR-018 | todo | Enviar solo lo solicitado |
| T-031 | Implementar contract tests de paridad automatizados | FR-017, NFR-009 | todo | Falla si falta campo/parametro |
| T-032 | Documentar politica "no bypass" para consumidores | FR-018, FR-012 | todo | Guia de uso avanzada |

## EPIC-08 Arquitectura y empaquetado NuGet
Objetivo: dejar una arquitectura limpia para paquete unico instalable (`GammaReportLibrary`) con dependencias transitivas y sin ciclos.

| ID | Tarea | Req | Estado | Notas |
|---|---|---|---|---|
| T-033 | Crear proyecto `GammaReportLibrary.Abstractions` | FR-021, NFR-002 | done | Contrato publico dedicado |
| T-034 | Mover interfaces publicas a `Abstractions` | FR-021 | done | Incluye `IGammaClient` y `IGammaRawClient` |
| T-035 | Mover opciones, DTOs y excepciones publicas a `Abstractions` | FR-021 | done | Contrato sin logica |
| T-036 | Ajustar referencias para grafo aciclico por capas | FR-023, NFR-010 | done | `Application` no depende de `Infrastructure` |
| T-037 | Reconfigurar `GammaReportLibrary` como paquete facade unico | FR-020, FR-022, NFR-011 | done | Punto de entrada de consumo + DI |
| T-038 | Validar instalacion en sample con paquete unico | FR-020, NFR-011 | done | Sample consume solo `GammaReportLibrary` en ProjectReference |
| T-039 | Eliminar adaptadores temporales innecesarios post-migracion | FR-022, NFR-002 | done | Se elimina `Infrastructure/GammaClient.cs` |
| T-040 | Ejecutar gate de arquitectura (sin ciclos + capas correctas) | FR-023, NFR-010 | done | `dotnet test` en verde y grafo aciclico |

## 3. Dependencias clave
- EPIC-08 debe ejecutarse primero para fijar la base de arquitectura y empaquetado.
- EPIC-01 debe completarse antes de EPIC-02, EPIC-03 y EPIC-04.
- T-009 depende de T-006 o T-008 para pruebas de flujo completo.
- EPIC-06 depende de avances minimos en EPIC-02 a EPIC-05.
- EPIC-07 cruza todas las epicas funcionales y debe cerrarse antes de release estable `v1.0.0`.
- Durante esta fase (pre-release), las tareas pueden introducir cambios estructurales sin estrategia de compatibilidad retroactiva.

## 4. Definicion de terminado (DoD)
- Codigo compilable en .NET 10 en macOS.
- Pruebas relevantes en verde.
- Documentacion actualizada.
- Requerimientos trazados en tarea.
- Cambios de API publica revisados.
- Dependencias de proyectos sin ciclos y acordes a arquitectura objetivo.
- Validacion de consumo via paquete NuGet unico completada.

## 5. Registro de cambios del backlog
| Fecha | Cambio | Autor |
|---|---|---|
| 2026-06-24 | Creacion inicial de backlog y epicas | Copilot |
| 2026-06-25 | Se agrega EPIC-07 para paridad total y politica no bypass | Copilot |
| 2026-07-04 | Se agrega EPIC-08 (arquitectura/NuGet) y politica greenfield sin compatibilidad retroactiva pre-release | Copilot |
