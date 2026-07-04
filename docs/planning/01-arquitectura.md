# Arquitectura objetivo de la libreria cliente Gamma API (.NET 10)

## 1. Objetivo

Publicar un solo paquete NuGet instalable por el consumidor (`GammaReportLibrary`) que traiga todo lo necesario por dependencias transitivas, manteniendo separacion limpia de capas y evitando ciclos entre proyectos.

## 2. Principios para NuGet

1. Experiencia de consumo simple
- El consumidor instala solo `GammaReportLibrary`.
- No debe instalar manualmente paquetes internos adicionales.

2. Sin ciclos de referencias
- No puede existir referencia circular entre proyectos.
- La composicion final debe respetar direccion unica de dependencias.

3. Clean Architecture aplicada al empaquetado
- La separacion por capas se mantiene internamente.
- La fachada publica se entrega como un paquete unico.

## 3. Estructura propuesta de proyectos

1. `GammaReportLibrary.Abstractions` (nuevo)
- Contrato publico del SDK: interfaces, opciones, DTOs publicos y excepciones publicas.
- No contiene logica de negocio ni detalles tecnicos.

2. `GammaReportLibrary.Domain`
- Reglas de negocio core, estados y tipos de dominio reutilizables.
- No depende de otros proyectos.
- Estado actual: proyecto reservado y fuera de referencias activas hasta incorporar codigo de dominio real.

3. `GammaReportLibrary.Application`
- Casos de uso y orquestacion.
- Depende de `Domain` y `Abstractions`.
- Nunca depende de `Infrastructure`.

4. `GammaReportLibrary.Infrastructure`
- HTTP, serializacion, resiliencia y adaptadores tecnicos.
- Depende de `Application`, `Domain` y `Abstractions`.

5. `GammaReportLibrary` (paquete facade)
- Proyecto orientado a empaquetado y experiencia de consumo.
- Depende de `Abstractions`, `Application` e `Infrastructure`.
- Expone punto de entrada de DI para consumidor.

## 4. Flujo de dependencias esperado

```text
GammaReportLibrary (NuGet unico)
        |
        v
  Infrastructure ---> Application ---> Domain
         \               ^
          \              |
           +---------- Abstractions
```

Notas:
- `Abstractions` es compartido por `Application` e `Infrastructure` para evitar que el contrato publico viva en un proyecto con implementacion.
- `GammaReportLibrary` actua como paquete agregador/fachada y no debe introducir logica de negocio.

## 5. Distribucion de responsabilidades

1. Contratos publicos
- `IGammaClient`, `IGammaRawClient`, `GammaClientOptions`, DTOs y excepciones publicas en `Abstractions`.

2. Core de dominio
- Reglas y tipos internos de negocio en `Domain`.

3. Orquestacion
- Casos de uso en `Application`.

4. Integracion tecnica
- Transporte HTTP, retry, parseo de headers y registro DI tecnico en `Infrastructure`.

5. Entrada para consumidor
- Paquete `GammaReportLibrary` con extension DI y referencias transitivas necesarias.

## 6. Reglas de validacion

1. Regla principal
- `Application -> Infrastructure` esta prohibido.

2. Reglas permitidas
- `Infrastructure -> Application` es valido.
- `Application -> Domain` es obligatorio para casos de uso.
- `Infrastructure -> Domain` es valido cuando requiere tipos/reglas de dominio.

3. Regla de paquete
- El consumidor instala solo `GammaReportLibrary`.
- El paquete debe resolver transitivamente la implementacion completa del SDK.

4. Regla de fachada
- `GammaReportLibrary` no implementa casos de uso.
- `GammaReportLibrary` no implementa transporte HTTP.

## 7. Estado minimo aceptable para continuar

1. Compila toda la solucion sin ciclos de referencia.
2. Tests existentes en verde.
3. Consumidor instala un solo paquete (`GammaReportLibrary`).
4. Casos de uso en `Application`.
5. Transporte/resiliencia en `Infrastructure`.
6. Contrato publico concentrado en `Abstractions`.

## 8. Plan de migracion sugerido (si se aprueba)

1. Crear proyecto `GammaReportLibrary.Abstractions`.
2. Mover contratos publicos actuales (interfaces, opciones, DTOs y excepciones publicas) a `Abstractions`.
3. Ajustar referencias:
- `Application -> Domain + Abstractions`
- `Infrastructure -> Application + Domain + Abstractions`
- `GammaReportLibrary -> Abstractions + Application + Infrastructure`
4. Dejar en `GammaReportLibrary` solo superficie facade orientada a consumo y empaquetado.
5. Ejecutar `dotnet test` y validar instalacion tipo consumidor (sample) usando un solo paquete.
6. Con validacion aprobada, mantener este contenido como referencia oficial de arquitectura.
