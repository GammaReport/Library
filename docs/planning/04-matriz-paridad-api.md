# Matriz de paridad API - GammaReportLibrary

## 1. Objetivo
Asegurar paridad 1:1 entre Gamma Public API y la superficie publica de la libreria.

Regla base:
- Ningun endpoint, parametro o campo de respuesta puede quedar sin exponer.
- Todo parametro opcional debe poder enviarse por el consumidor.

## 2. Como usar esta matriz
- `Cobertura SDK`: indica donde quedara expuesto.
  - `Typed`: API tipada principal (`IGammaClient`).
  - `Raw`: acceso de bajo nivel (`IGammaRawClient`).
  - `Both`: debe estar en ambas.
- `Estado`: `todo`, `in-progress`, `done`.
- `Req`: vinculo con FR/NFR en [02-requerimientos.md](02-requerimientos.md).

## 2.1 Estado actual de cobertura

Revision manual realizada sobre los modelos tipados y contratos publicos existentes.

| Area | Estado actual | Observacion |
|---|---|---|
| Endpoints base | done | Los 7 endpoints planificados ya tienen metodo en `IGammaClient` y via raw en `IGammaRawClient`. |
| Requests tipados de generations | done | Se exponen todos los campos documentados en la matriz, incluyendo `AdditionalParameters` para forward-compatibility. |
| Requests tipados from-template | done | Se exponen todos los campos documentados actualmente en la matriz. |
| Responses tipadas principales | done | Se preserva `RawBody` en respuestas tipadas y `RawGammaResponse` en la capa raw. |
| Paginacion themes/folders | done | `limit`, `after`, `query` y `type` aplicable ya estan modelados. |
| Mapeo de errores HTTP | done | `400`, `401`, `402`, `403`, `404`, `429`, `500+` mapeados a excepciones de dominio. |
| Retry de transitorios | done | `429` y `5xx` con backoff y soporte de `Retry-After`. |
| Comentarios XML | in-progress | La superficie critica ya tiene comentarios; falta completar todos los miembros publicos restantes antes de retirar `CS1591`. |
| Paridad automatizada campo a campo | in-progress | Ya existen contract tests relevantes, pero no hay aun una prueba dedicada para cada propiedad anidada de serializacion. |

## 2.2 Gaps abiertos de paridad fina

- Completar comentarios XML en todos los miembros publicos para que IntelliSense cubra la totalidad de la superficie expuesta.
- Agregar pruebas de serializacion por propiedad anidada para `cardOptions.headerFooter`, `sharingOptions` y bolsas `AdditionalParameters`.
- Revisar periodicamente la documentacion de Gamma por cambios de contrato y reflejarlos en esta matriz y en los DTOs tipados.

## 3. Endpoints

| Endpoint | Metodo | Debe estar en SDK | Cobertura SDK | Estado | Req |
|---|---|---|---|---|---|
| /v1.0/generations | POST | Si | Both | todo | FR-013 |
| /v1.0/generations/from-template | POST | Si | Both | todo | FR-013 |
| /v1.0/generations/{id} | GET | Si | Both | todo | FR-013 |
| /v1.0/themes | GET | Si | Both | todo | FR-013 |
| /v1.0/folders | GET | Si | Both | todo | FR-013 |
| /v1.0/gammas/{gammaId}/archive | POST | Si | Both | todo | FR-013 |
| /v1.0/gammas/{gammaId} | DELETE | Si | Both | todo | FR-013 |

## 4. Parametros comunes HTTP

| Elemento | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| Header: X-API-KEY | string | Si | Both | todo | FR-001 | Autenticacion obligatoria |
| Header: Content-Type=application/json | string | Si en POST | Both | todo | FR-001 | Para requests JSON |
| Header rate-limit (x-ratelimit-*) | headers | No | Both | todo | FR-011, FR-017 | Deben quedar accesibles al consumidor |

## 5. POST /v1.0/generations - Request

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| inputText | string | Si | Both | todo | FR-014 | Limites segun API |
| title | string | No | Both | todo | FR-014 | 1-500 |
| textMode | enum | No | Both | todo | FR-014 | generate/condense/preserve |
| format | enum | No | Both | todo | FR-014 | presentation/document/social/webpage |
| numCards | integer | No | Both | todo | FR-014 | Segun plan |
| cardSplit | enum | No | Both | todo | FR-014 | auto/inputTextBreaks |
| themeId | string | No | Both | todo | FR-014 | |
| folderIds | array[string] | No | Both | todo | FR-014 | |
| additionalInstructions | string | No | Both | todo | FR-014 | |
| exportAs | enum | No | Both | todo | FR-014 | pdf/pptx/png |
| textOptions.amount | enum | No | Both | todo | FR-014 | brief/medium/detailed/extensive |
| textOptions.tone | string | No | Both | todo | FR-014 | |
| textOptions.audience | string | No | Both | todo | FR-014 | |
| textOptions.language | string | No | Both | todo | FR-014 | locale code |
| imageOptions.source | enum | No | Both | todo | FR-014 | aiGenerated/web*/pictographic/giphy/pexels/themeAccent/placeholder/noImages |
| imageOptions.model | string | No | Both | todo | FR-014 | |
| imageOptions.stylePreset | enum | No | Both | todo | FR-014 | photorealistic/illustration/abstract/3D/lineArt/custom |
| imageOptions.style | string | No | Both | todo | FR-014 | |
| cardOptions.dimensions | enum | No | Both | todo | FR-014 | Dependiente de format |
| cardOptions.headerFooter.topLeft.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.topLeft.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.topLeft.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.topLeft.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.topLeft.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.topCenter.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.topCenter.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.topCenter.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.topCenter.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.topCenter.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.topRight.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.topRight.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.topRight.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.topRight.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.topRight.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.bottomLeft.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.bottomLeft.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.bottomLeft.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.bottomLeft.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.bottomLeft.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.bottomCenter.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.bottomCenter.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.bottomCenter.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.bottomCenter.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.bottomCenter.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.bottomRight.type | enum | No | Both | todo | FR-014 | cardNumber/image/text |
| cardOptions.headerFooter.bottomRight.source | enum | No | Both | todo | FR-014 | themeLogo/custom |
| cardOptions.headerFooter.bottomRight.src | string | No | Both | todo | FR-014 | Si source=custom |
| cardOptions.headerFooter.bottomRight.value | string | No | Both | todo | FR-014 | Si type=text |
| cardOptions.headerFooter.bottomRight.size | enum | No | Both | todo | FR-014 | sm/md/lg/xl |
| cardOptions.headerFooter.hideFromFirstCard | boolean | No | Both | todo | FR-014 | |
| cardOptions.headerFooter.hideFromLastCard | boolean | No | Both | todo | FR-014 | |
| sharingOptions.workspaceAccess | enum | No | Both | todo | FR-014 | noAccess/view/comment/edit/fullAccess |
| sharingOptions.externalAccess | enum | No | Both | todo | FR-014 | noAccess/view/comment/edit |
| sharingOptions.emailOptions.recipients | array[string] | No | Both | todo | FR-014 | |
| sharingOptions.emailOptions.access | enum | No | Both | todo | FR-014 | view/comment/edit/fullAccess |

## 6. POST /v1.0/generations - Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| generationId | string | Si | Both | todo | FR-017 | |
| status | enum | No | Both | todo | FR-017 | pending esperado |
| gammaUrl | string | No | Both | todo | FR-017 | Puede no venir en create inicial |
| warnings | string | No | Both | todo | FR-017 | Si aplica |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 7. POST /v1.0/generations/from-template - Request

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| gammaId | string | Si | Both | todo | FR-015 | Template file ID |
| prompt | string | Si | Both | todo | FR-015 | |
| title | string | No | Both | todo | FR-015 | |
| themeId | string | No | Both | todo | FR-015 | |
| folderIds | array[string] | No | Both | todo | FR-015 | |
| exportAs | enum | No | Both | todo | FR-015 | pdf/pptx/png |
| imageOptions.model | string | No | Both | todo | FR-015 | |
| imageOptions.style | string | No | Both | todo | FR-015 | |
| sharingOptions.workspaceAccess | enum | No | Both | todo | FR-015 | noAccess/view/comment/edit/fullAccess |
| sharingOptions.externalAccess | enum | No | Both | todo | FR-015 | noAccess/view/comment/edit |
| sharingOptions.emailOptions.recipients | array[string] | No | Both | todo | FR-015 | |
| sharingOptions.emailOptions.access | enum | No | Both | todo | FR-015 | view/comment/edit/fullAccess |

## 8. POST /v1.0/generations/from-template - Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| generationId | string | Si | Both | todo | FR-017 | |
| status | enum | No | Both | todo | FR-017 | pending esperado |
| gammaUrl | string | No | Both | todo | FR-017 | |
| warnings | string | No | Both | todo | FR-017 | |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 9. GET /v1.0/generations/{id} - Path/Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| path.generationId | string | Si | Both | todo | FR-004 | |
| generationId | string | Si | Both | todo | FR-017 | |
| status | enum | Si | Both | todo | FR-017 | pending/completed/failed |
| gammaId | string | No | Both | todo | FR-017 | completed |
| gammaUrl | string | No | Both | todo | FR-017 | completed |
| exportUrl | string | No | Both | todo | FR-017 | completed y segun exportAs |
| credits.deducted | integer | No | Both | todo | FR-017 | completed/failed segun docs |
| credits.remaining | integer | No | Both | todo | FR-017 | completed/failed segun docs |
| error | object | No | Both | todo | FR-017 | failed |
| error.message | string | No | Both | todo | FR-017 | si viene en payload |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 10. GET /v1.0/themes - Query/Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| query.limit | integer | No | Both | todo | FR-016 | 1-50 |
| query.after | string | No | Both | todo | FR-016 | cursor |
| query.query | string | No | Both | todo | FR-016 | busqueda por nombre |
| query.type | enum | No | Both | todo | FR-016 | standard/custom |
| data[] | array | Si | Both | todo | FR-017 | |
| data[].id | string | Si | Both | todo | FR-017 | |
| data[].name | string | Si | Both | todo | FR-017 | |
| data[].type | enum | No | Both | todo | FR-017 | standard/custom |
| data[].colorKeywords | array[string] | No | Both | todo | FR-017 | |
| data[].toneKeywords | array[string] | No | Both | todo | FR-017 | |
| hasMore | boolean | Si | Both | todo | FR-017 | |
| nextCursor | string|null | Si | Both | todo | FR-017 | |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 11. GET /v1.0/folders - Query/Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| query.limit | integer | No | Both | todo | FR-016 | 1-50 |
| query.after | string | No | Both | todo | FR-016 | cursor |
| query.query | string | No | Both | todo | FR-016 | busqueda por nombre |
| data[] | array | Si | Both | todo | FR-017 | |
| data[].id | string | Si | Both | todo | FR-017 | |
| data[].name | string | Si | Both | todo | FR-017 | |
| hasMore | boolean | Si | Both | todo | FR-017 | |
| nextCursor | string|null | Si | Both | todo | FR-017 | |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 12. POST /v1.0/gammas/{gammaId}/archive - Path/Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| path.gammaId | string | Si | Both | todo | FR-008 | File ID API |
| gammaId | string | Si | Both | todo | FR-017 | response |
| archived | boolean | Si | Both | todo | FR-017 | response |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 13. DELETE /v1.0/gammas/{gammaId} - Path/Response

| Campo | Tipo | Requerido | Cobertura SDK | Estado | Req | Notas |
|---|---|---|---|---|---|---|
| path.gammaId | string | Si | Both | todo | FR-009 | File ID API |
| status | enum | Si | Both | todo | FR-017 | deleted |
| gammaId | string | Si | Both | todo | FR-017 | |
| message | string | Si | Both | todo | FR-017 | |
| Raw body completo | object | Si | Raw | todo | FR-019 | Sin perdida de informacion |

## 14. Mapeo de errores

| HTTP | Debe mapearse en SDK tipado | Debe preservarse en Raw | Estado | Req |
|---|---|---|---|---|
| 400 | Si | Si | todo | FR-010 |
| 401 | Si | Si | todo | FR-010 |
| 402 | Si | Si | todo | FR-010 |
| 403 | Si | Si | todo | FR-010 |
| 404 | Si | Si | todo | FR-010 |
| 429 | Si | Si | todo | FR-010 |
| 500 | Si | Si | todo | FR-010 |
| 502 | Si | Si | todo | FR-010 |

## 15. Criterio de cierre de paridad
Se considera paridad completa cuando:
1. Todas las filas de la matriz estan en `done`.
2. Existen contract tests automatizados para request/response de cada endpoint.
3. Ningun campo documentado por Gamma queda inaccesible desde `IGammaClient` o `IGammaRawClient`.
4. La guia de uso documenta como enviar parametros opcionales avanzados.

## 16. Registro de cambios

| Fecha | Cambio | Autor |
|---|---|---|
| 2026-06-25 | Creacion inicial de matriz de paridad | Copilot |
