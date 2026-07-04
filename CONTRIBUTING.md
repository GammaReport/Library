# Contributing to GammaReportLibrary

Gracias por contribuir a este proyecto.

## Alcance

Este repositorio contiene la libreria principal de GammaReportLibrary y su arquitectura por capas.

## Requisitos

1. .NET SDK 10 instalado.
2. Git configurado.
3. Restauracion de paquetes funcionando (`dotnet restore`).

## Flujo recomendado

1. Crea una rama desde `main`.
2. Implementa cambios pequenos y enfocados.
3. Agrega o actualiza pruebas.
4. Ejecuta build y tests localmente.
5. Actualiza documentacion si cambias comportamiento publico.
6. Abre Pull Request con descripcion clara.

## Estandares de codigo

1. Mantener separacion por capas definida en `docs/planning/01-arquitectura.md`.
2. Evitar acoplamientos fuera de la direccion de dependencias acordada.
3. No introducir valores sensibles en codigo fuente.
4. Preferir nombres de pruebas descriptivos y eliminar placeholders vacios.

## Validacion local minima

Ejecuta antes de subir cambios:

```bash
dotnet build GammaReportLibrary.slnx
dotnet test GammaReportLibrary.slnx
```

## Pruebas

1. `tests/GammaReportLibrary.UnitTests`: pruebas unitarias.
2. `tests/GammaReportLibrary.ContractTests`: pruebas de contrato y mapeo.

Si agregas una funcionalidad nueva, incluye al menos una prueba relevante.

## Documentacion

Cuando cambies arquitectura, requerimientos o roadmap, actualiza:

1. `docs/planning/01-arquitectura.md`
2. `docs/planning/02-requerimientos.md`
3. `docs/planning/03-backlog.md`
4. `docs/planning/04-matriz-paridad-api.md` (si aplica)

## Seguridad y datos sensibles

1. No subir API keys, tokens ni datos de clientes.
2. No incluir prompts o payloads sensibles en ejemplos publicos.
3. Si necesitas samples con datos sensibles, mantenlos fuera de este repositorio publico.

## Commits y PR

1. Usa mensajes de commit claros y orientados a accion.
2. En PR, incluye:
- Resumen de cambios.
- Riesgos o impactos.
- Evidencia de build/tests.

## Reporte de bugs

Incluye:

1. Comportamiento esperado vs actual.
2. Pasos para reproducir.
3. Version de .NET y sistema operativo.
4. Logs o mensajes de error relevantes.
