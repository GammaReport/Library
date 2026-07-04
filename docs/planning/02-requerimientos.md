# Requerimientos de la libreria GammaReportLibrary

## 1. Objetivo de requerimientos
Definir requerimientos funcionales y no funcionales trazables para construir una libreria cliente de Gamma API reutilizable en .NET 10.

## 2. Requerimientos funcionales (FR)

### FR-001 Autenticacion API key
- La libreria debe enviar `X-API-KEY` en todas las requests.
- Criterio de aceptacion: toda invocacion HTTP incluye header requerido.

### FR-002 Crear generacion desde texto
- Exponer operacion para `POST /v1.0/generations`.
- Criterio de aceptacion: retorna al menos `generationId` y estado inicial.

### FR-003 Crear generacion desde template
- Exponer operacion para `POST /v1.0/generations/from-template`.
- Criterio de aceptacion: acepta `gammaId` + `prompt` y retorna `generationId`.

### FR-004 Consultar estado de generacion
- Exponer operacion para `GET /v1.0/generations/{id}`.
- Criterio de aceptacion: mapea `pending`, `completed`, `failed`.

### FR-005 Polling de completion
- Exponer helper para esperar completion con intervalo configurable y cancelacion.
- Criterio de aceptacion: termina en `completed`, `failed`, timeout o cancelacion.

### FR-006 Listar themes
- Exponer operacion para `GET /v1.0/themes` con paginacion.
- Criterio de aceptacion: soporta `limit` y `after`.

### FR-007 Listar folders
- Exponer operacion para `GET /v1.0/folders` con paginacion.
- Criterio de aceptacion: soporta `limit` y `after`.

### FR-008 Archivar gamma
- Exponer operacion para `POST /v1.0/gammas/{gammaId}/archive`.
- Criterio de aceptacion: retorna confirmacion `archived`.

### FR-009 Eliminar gamma
- Exponer operacion para `DELETE /v1.0/gammas/{gammaId}`.
- Criterio de aceptacion: retorna estado `deleted`.

### FR-010 Mapeo de errores HTTP
- Mapear errores conocidos (`400`, `401`, `402`, `403`, `404`, `429`, `5xx`) a excepciones/tipos de error de dominio.
- Criterio de aceptacion: consumers pueden distinguir causas y reaccionar.

### FR-011 Exponer metadatos de rate limit
- Permitir acceso a headers de rate limit en responses relevantes.
- Criterio de aceptacion: consumer puede ajustar estrategia de polling.

### FR-012 API publica documentada
- Incluir comentarios XML en metodos y tipos publicos para IntelliSense en IDE.
- Publicar documentacion de consumo legible en Markdown (README y referencia) con ejemplos de uso.
- Criterio de aceptacion: al invocar metodos de la libreria en el IDE se visualiza descripcion util y, ademas, existe guia de uso legible fuera del codigo.

### FR-013 Paridad total de endpoints
- La libreria debe exponer todos los endpoints disponibles de Gamma API en su superficie publica.
- Criterio de aceptacion: checklist de endpoints 100% cubierto.

### FR-014 Exposicion total de parametros en `/generations`
- La libreria debe exponer todos los parametros del endpoint `POST /v1.0/generations` (obligatorios y opcionales).
- Criterio de aceptacion: ningun parametro documentado queda fuera del contrato publico.

### FR-015 Exposicion total de parametros en `/generations/from-template`
- La libreria debe exponer todos los parametros del endpoint `POST /v1.0/generations/from-template` (obligatorios y opcionales).
- Criterio de aceptacion: ningun parametro documentado queda fuera del contrato publico.

### FR-016 Exposicion total de filtros/paginacion en listados
- La libreria debe exponer todos los query params soportados por `GET /themes` y `GET /folders`.
- Criterio de aceptacion: soporte completo de `limit`, `after`, `query` y filtros aplicables por endpoint.

### FR-017 Exposicion total de campos de respuesta
- La libreria debe exponer todos los campos de respuesta documentados por endpoint, incluyendo warnings, credits y objetos de error.
- Criterio de aceptacion: tests de contrato validan mapeo de campos sin perdida.

### FR-018 Politica de no bypass
- La libreria no debe ocultar ni descartar opciones de la API por simplificacion del SDK.
- Criterio de aceptacion: auditoria de API surface confirma que todo parametro opcional puede ser enviado por el consumidor.

### FR-019 Forward-compatibility controlada
- La libreria debe permitir un camino de acceso de bajo nivel para escenarios avanzados donde el consumidor requiera control total del request/response.
- Criterio de aceptacion: contrato `IGammaRawClient` disponible y documentado.

### FR-020 Paquete NuGet unico para consumidor
- El consumidor debe instalar solo `GammaReportLibrary` para disponer de toda la funcionalidad del SDK.
- Criterio de aceptacion: instalando `GammaReportLibrary` se resuelven transitivamente los ensamblados necesarios sin instalar paquetes internos manualmente.

### FR-021 Separacion de contrato publico en `Abstractions`
- Los contratos publicos del SDK (interfaces, opciones, DTOs y excepciones publicas) deben concentrarse en un proyecto dedicado `GammaReportLibrary.Abstractions`.
- Criterio de aceptacion: `Application` e `Infrastructure` consumen contratos desde `Abstractions` y no desde un proyecto con implementacion.

### FR-022 Fachada de entrada para consumo
- `GammaReportLibrary` debe actuar como fachada de empaquetado y punto de entrada de consumo (incluyendo extension DI), sin contener logica de casos de uso ni transporte.
- Criterio de aceptacion: el consumidor configura servicios desde la fachada y la logica/orquestacion permanece en `Application`.

### FR-023 Direccion de dependencias por capa
- Debe cumplirse la direccion de dependencias definida por arquitectura (`Application` no puede depender de `Infrastructure`).
- Criterio de aceptacion: referencias de proyectos verificadas sin dependencias prohibidas.

## 3. Requerimientos no funcionales (NFR)

### NFR-001 Plataforma
- Debe compilar y ejecutar en .NET 10.

### NFR-002 Mantenibilidad
- Seguir Clean Architecture y separacion por responsabilidades.

### NFR-003 Testabilidad
- Cobertura minima objetivo: 80% en capa Application y Domain.

### NFR-004 Rendimiento
- No bloquear hilos con operaciones sincronas de red.

### NFR-005 Seguridad
- No exponer API key en logs o mensajes de error.

### NFR-006 Reutilizacion
- API publica estable y facil de integrar por DI.

### NFR-007 Observabilidad
- Logging estructurado opcional para request lifecycle sin secretos.

### NFR-008 Evolucion
- Versionado semantico y changelog mantenido.

### NFR-009 Cobertura de paridad verificable
- Debe existir una matriz de paridad endpoint-parametro-campo mantenida junto al codigo y validada por pruebas.

### NFR-010 Sin ciclos de dependencias
- La solucion no debe contener referencias circulares entre proyectos.
- Criterio de aceptacion: restauracion/compilacion exitosa con grafo de dependencias aciclico.

### NFR-011 Experiencia de consumo del paquete
- La experiencia de instalacion y uso del NuGet principal debe ser directa, con onboarding minimo.
- Criterio de aceptacion: sample de consumo funcionando instalando un unico paquete.

## 4. Supuestos
- El endpoint base se mantiene en `https://public-api.gamma.app/v1.0`.
- Los contratos de Gamma pueden evolucionar; la libreria requiere estrategia de compatibilidad.
- No se implementara edicion de gammas existentes porque la API no lo soporta.

## 5. Criterios globales de aceptacion
- Solucion compila con .NET 10.
- Tests unitarios y de contrato pasan en pipeline local.
- API publica documentada y empaquetable como NuGet.
- Instalacion de `GammaReportLibrary` resuelve funcionalidad completa por dependencias transitivas.
- Grafo de referencias de proyectos sin ciclos y conforme a capas definidas.
- Trazabilidad completa FR/NFR hacia tareas del backlog.
- Paridad comprobable 1:1 entre documentacion de Gamma API y superficie publica de la libreria.

## 6. Matriz de trazabilidad inicial
| Requerimiento | Epic backlog sugerida |
|---|---|
| FR-001, FR-010 | EPIC-01 Transporte y autenticacion |
| FR-002, FR-003, FR-004, FR-005 | EPIC-02 Generaciones y polling |
| FR-006, FR-007 | EPIC-03 Recursos de workspace |
| FR-008, FR-009 | EPIC-04 Gestion de gammas |
| FR-011, NFR-004, NFR-007 | EPIC-05 Resiliencia y observabilidad |
| FR-012, NFR-002, NFR-003, NFR-006, NFR-008 | EPIC-06 Calidad, docs y publicacion |
| FR-013, FR-014, FR-015, FR-016, FR-017, FR-018, FR-019, NFR-009 | EPIC-07 Paridad completa API surface |
| FR-020, FR-021, FR-022, FR-023, NFR-010, NFR-011 | EPIC-08 Arquitectura y empaquetado NuGet |
