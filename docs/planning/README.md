# Plan de trabajo vivo

Este directorio contiene los artefactos base para planificar la construccion de la libreria cliente Gamma API con enfoque incremental.

## Orden de trabajo recomendado
1. Arquitectura: [01-arquitectura.md](01-arquitectura.md)
2. Requerimientos: [02-requerimientos.md](02-requerimientos.md)
3. Backlog y ejecucion: [03-backlog.md](03-backlog.md)
4. Matriz de paridad API: [04-matriz-paridad-api.md](04-matriz-paridad-api.md)

## Regla de actualizacion
- Si cambia una decision estructural: actualizar primero arquitectura.
- Si cambia alcance funcional/no funcional: actualizar requerimientos.
- Si cambia la implementacion planificada: actualizar backlog.
- Si cambia el contrato de Gamma API: actualizar la matriz de paridad.

## Convencion de trazabilidad
- Requerimientos: `FR-###` y `NFR-###`.
- Epicas: `EPIC-##`.
- Tareas: `T-###`.

## Proxima fase
Con estos documentos validados, la siguiente fase es crear el esqueleto de solucion .NET 10 y ejecutar las tareas por epica.
