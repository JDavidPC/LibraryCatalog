# Library Catalog

Sistema de consulta del catálogo de una biblioteca. Permite consultar **libros**, **autores** y **categorías** ya registrados en el catálogo.

> Primera versión: solo lectura. No se registra, modifica ni elimina información; el objetivo es construir la infraestructura necesaria para consultar el catálogo existente.

## Stack

| Tema | Tecnología |
| --- | --- |
| Plataforma | .NET 10 / ASP.NET Core Web API |
| Arquitectura | Clean Architecture + Domain-Driven Design (DDD) |
| Patrón de casos de uso | CQRS (queries) con un mediator propio (`SimpleMediator`) |
| Persistencia | SQL Server + Entity Framework Core 10 (**Code First**) |
| Pruebas | MSTest |

## Arquitectura

```
Catalog.Api ─────────────► Catalog.Application ─────────► Catalog.Domain
    │                              ▲                             ▲
    └──► Catalog.Persistence ──────┘ (implementa contratos)      │
                   └───────────────────────────────────────────┘
```

| Proyecto | Capa | Responsabilidad |
| --- | --- | --- |
| `Catalog.Domain` | Core | Entidades, value objects (`Isbn`) y reglas de negocio (`BusinessRuleException`). Sin dependencias. |
| `Catalog.Application` | Core | Contratos de repositorios, casos de uso (queries), DTOs, mediator y paginación. |
| `Catalog.Persistence` | Infrastructure | `DataContext`, configuraciones Fluent API, repositorios, seeders y migraciones. |
| `Catalog.Api` | Presenters | Controllers, manejo global de errores y composición de dependencias. |
| `Catalog.Tests` | Tests | Pruebas unitarias de dominio, casos de uso y mediator. |

Regla de dependencias: las capas internas **no conocen** a las externas. `Application` define las interfaces (`IBooksRepository`, `ICategoriesRepository`) y `Persistence` las implementa.

## Modelo de dominio

```
Author 1 ───── * Book * ───── 1 Category
```

| Entidad | Propiedades |
| --- | --- |
| `Book` | `Id`, `Title`, `Isbn` (value object), `PublicationYear`, `Description?`, `AuthorId`, `CategoryId` |
| `Author` | `Id`, `FirstName`, `LastName`, `Nationality?`, `FullName` (calculada) |
| `Category` | `Id`, `Name`, `Description?` |

Reglas destacadas (se validan en el constructor mediante métodos `Apply...Rules`):

- `Isbn` es un value object: acepta guiones/espacios, exige 13 dígitos y valida el dígito de control ISBN-13.
- El año de publicación debe estar entre 1000 y el año actual.
- El título es obligatorio (máx. 256). Autor y categoría son obligatorios.
- Los `Id` son `Guid` v7 generados por el dominio.

### Esquema generado por Code First

| Tabla | Columnas clave | Restricciones |
| --- | --- | --- |
| `Books` | `Id`, `Title(256)`, `Isbn(char 13)`, `PublicationYear`, `Description(2048)`, `AuthorId`, `CategoryId` | `Isbn` único · FK a `Authors` y `Categories` (`Restrict`) |
| `Authors` | `Id`, `FirstName(64)`, `LastName(64)`, `Nationality(64)` | |
| `Categories` | `Id`, `Name(64)`, `Description(512)` | `Name` único |

## Casos de uso (CQRS)

Cada caso de uso vive en `Catalog.Application/UseCases/Books/Queries/<NombreDelCasoDeUso>` y está compuesto por su `Query`, su `UseCase` (handler) y su DTO.

| # | Query | Endpoint | Resultado |
| --- | --- | --- | --- |
| 1 | `GetBooksListQuery` | `GET /api/books?pageNumber=1&pageSize=15` | `PaginationResponse<BookListItemDTO>`: Id, título, ISBN, año, autor, categoría |
| 2 | `GetBookByIdQuery` | `GET /api/books/{id}` | `BookDetailDTO`: datos del libro + autor (nombre, nacionalidad) + categoría (nombre, descripción). `404` si no existe |
| 3 | `GetBooksByCategoryQuery` | `GET /api/books/category/{categoryId}?pageNumber=1&pageSize=15` | `PaginationResponse<BookListItemDTO>`. `404` si la categoría no existe |

Paginación: `pageSize` por defecto 15, máximo 50. Los listados se ordenan por título.

Ejemplo de respuesta (Query 1):

```json
{
  "items": [
    {
      "id": "0198...",
      "title": "Cien años de soledad",
      "isbn": "9780307474728",
      "publicationYear": 1967,
      "authorId": "0198...",
      "authorName": "Gabriel García Márquez",
      "categoryId": "0198...",
      "categoryName": "Novela"
    }
  ],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 15,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

Errores: `NotFoundException` → `404`, `BusinessRuleException` → `400`, cualquier otro → `500`, todos como `ProblemDetails` (ver `Handlers/GlobalExceptionHandler.cs`).

## Cómo ejecutar

### Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (Express, Developer o LocalDB)
- Herramienta de EF Core: `dotnet tool install --global dotnet-ef`

### 1. Configurar la cadena de conexión

Editar `ConnectionStrings:MyConnection` en `Catalog.Api/appsettings.json` (por defecto usa `Server=localhost\SQLEXPRESS` con autenticación de Windows).

### 2. Aplicar migraciones y crear la base de datos (Code First)

Si clonas el repositorio por primera vez, la migración inicial ya viene incluida, así que normalmente solo necesitas aplicar la base de datos:

```bash
ef.cmd update
```

Si haces cambios al modelo y necesitas generar una migración nueva, usa:

```bash
ef.cmd add NombreMigracion
```

O directamente con la CLI:

```bash
dotnet ef database update --project Catalog.Persistence --startup-project Catalog.Api
```

### 3. Ejecutar la API

```bash
dotnet run --project Catalog.Api
```

Al iniciar, `DataBaseSeeder` carga datos de ejemplo (4 categorías, 7 autores, 10 libros) solo si las tablas están vacías. Las peticiones de prueba están en `Catalog.Api/Catalog.Api.http`.

La especificación OpenAPI está disponible en `/openapi/v1.json` (entorno Development).
La interfaz de Swagger UI está en `/swagger` o `/swagger/index.html`.

### 4. Ejecutar las pruebas

```bash
dotnet test
```

## Estructura de carpetas

```
Catalog.slnx
ef.cmd
Catalog.Domain/
├── Entities/{Authors, Books, Categories}
│   └── Books/ValueObjects/Isbn.cs
└── Exceptions/BusinessRuleException.cs
Catalog.Application/
├── Contracts/Repositories/
├── Exceptions/
├── UseCases/Books/{Common, Queries/{GetBooksList, GetBookById, GetBooksByCategory}}
├── Utilities/{Mediator, Pagination}
└── ApplicationServicesRegistry.cs
Catalog.Persistence/
├── Configurations/
├── Extensions/
├── Repositories/
├── Seeds/
├── DataContext.cs
└── PersistenceServicesRegistry.cs
Catalog.Api/
├── Controllers/BooksController.cs
├── Handlers/GlobalExceptionHandler.cs
└── Program.cs
Catalog.Tests/
```

## Flujo de trabajo con Git

El desarrollo se gestiona con commits pequeños por capa/funcionalidad:

```
chore: init solution and gitignore
feature(catalog): create domain layer
feature(catalog): create application layer with book queries
feature(catalog): create persistence layer with ef core code first
feature(catalog): create api layer
test(catalog): add unit tests
feature(catalog): add initial migration
docs: add readme
```
